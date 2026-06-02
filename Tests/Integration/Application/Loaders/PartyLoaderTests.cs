namespace Tests.Integration.Application.Loaders;

using System;
using Xunit;
using Moq;
using Backend.Application.Loaders;
using Backend.Application.Loaders.Parties;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Parties;
using Backend.Memory.Readers.Parties.Digimons;

public class PartyLoaderTests : LoaderIntegrationTestBase
{
    [Fact]
    public void Load_ShouldIntegratePipelineAndSkipEmptySlots()
    {
        var addressesRepository = CreateAddressesRepository();

        // 2. Arrange - Setup mocks for lower level hardware memory reader
        var memoryReaderMock = new Mock<IMemoryReader>();

        // Simular a leitura dos 3 slots da equipe na RAM (endereços carregados do JSON real)
        // Slot 0 (Index 1) -> Endereço 0x00048DA4: Contém Digimon ID 1 (Kumamon)
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DA4, 4))
            .Returns([1, 0, 0, 0]);

        // Slot 1 (Index 2) -> Endereço 0x00048DA8: Contém ID de slot vazio 0xFF (255)
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DA8, 4))
            .Returns([0xFF, 0, 0, 0]);

        // Slot 2 (Index 3) -> Endereço 0x00048DAC: Contém ID de slot vazio 0xFF (255)
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DAC, 4))
            .Returns([0xFF, 0, 0, 0]);

        // Simular o bloco de memória física de 1500 bytes para o Kumamon (ID 1, Endereço 0x00049878)
        var fakeMemoryBlock = new byte[1500];

        // Mapear dados reais usando os offsets reais extraídos de DigimonStatusAddresses.json:
        // Experience (Int32) no offset 0x18 (24) -> 1500 XP
        BitConverter.GetBytes(1500).CopyTo(fakeMemoryBlock, 24);

        // Level (Int16) no offset 0x1C (28) -> Level 12
        BitConverter.GetBytes((short)12).CopyTo(fakeMemoryBlock, 28);

        // CurrentHP (Int16) no offset 0x20 (32) -> 450 HP
        BitConverter.GetBytes((short)450).CopyTo(fakeMemoryBlock, 32);

        // MaxHP (Int16) no offset 0x22 (34) -> 500 HP
        BitConverter.GetBytes((short)500).CopyTo(fakeMemoryBlock, 34);

        // Strength (Int16) no offset 0x28 (40) -> Strength 42
        BitConverter.GetBytes((short)42).CopyTo(fakeMemoryBlock, 40);

        // Digievolutions.Slots:
        // Slot 1 at offset 0x48 (72) -> ID 5
        BitConverter.GetBytes((short)5).CopyTo(fakeMemoryBlock, 72);
        // Slot 2 at offset 0x4A (74) -> ID 10
        BitConverter.GetBytes((short)10).CopyTo(fakeMemoryBlock, 74);

        // UnlockedDigievolutionsStart at offset 0x50 (80)
        // Configura evolução com ID 5 no nível 3
        BitConverter.GetBytes((short)5).CopyTo(fakeMemoryBlock, 80);
        BitConverter.GetBytes((short)3).CopyTo(fakeMemoryBlock, 82);

        // Configura evolução com ID 10 no nível 1 (não listada, retorna padrão 1)

        memoryReaderMock.Setup(m => m.ReadBytes(0x00049878, 1500))
            .Returns(fakeMemoryBlock);

        // ActiveDigievolution no offset -4 -> Active ID 5
        memoryReaderMock.Setup(m => m.ReadInt16(0x00049878 - 4))
            .Returns(5);

        // 3. Arrange - Instanciação da árvore de dependências reais (Pipeline Completo)
        var digievolutionSlotReader = new DigievolutionSlotReader();
        var digievolutionReader = new DigievolutionReader();
        var storedDigievolutionReader = new StoredDigievolutionReader();
        var digimonReader = new DigimonReader(memoryReaderMock.Object, digievolutionSlotReader, digievolutionReader, storedDigievolutionReader);
        var digimonSlotReader = new DigimonSlotReader(memoryReaderMock.Object);
        var partyReader = new PartyReader(digimonSlotReader);

        var digimonLoader = new DigimonLoader(addressesRepository, digimonReader);
        var partyLoader = new PartyLoader(addressesRepository, partyReader, digimonLoader);

        // 4. Act - Execução do Loader integrado
        var partyResource = partyLoader.Load();

        // 5. Assert - Validação integrada de ponta a ponta
        Assert.NotNull(partyResource);
        Assert.Equal(3, partyResource.SlotsResource.Count);

        // Validar Slot 0 (Kumamon carregado)
        var slot0 = partyResource.SlotsResource[0];
        Assert.Equal(1, slot0.Index);
        Assert.Equal(1, slot0.DigimonId);
        Assert.NotNull(slot0.DigimonResource);
        
        var kumamon = slot0.DigimonResource;
        Assert.Equal(5, kumamon.ActiveDigievolutionId);
        Assert.Equal(1500, kumamon.Experience);
        Assert.Equal(12, kumamon.Level);
        Assert.Equal(450, kumamon.Vitals.CurrentHP);
        Assert.Equal(500, kumamon.Vitals.MaxHP);
        Assert.Equal(42, kumamon.Attributes.Strength);

        // Validar que a árvore evolutiva de Kumamon integrou perfeitamente
        Assert.Equal(3, kumamon.Digievolutions.Count);
        
        var evolutionSlot1 = kumamon.Digievolutions[0];
        Assert.Equal(5, evolutionSlot1.DigievolutionId);
        Assert.NotNull(evolutionSlot1.DigievolutionResource);
        Assert.Equal(3, evolutionSlot1.DigievolutionResource.Level); // Destravado e encontrado em 0x50

        var evolutionSlot2 = kumamon.Digievolutions[1];
        Assert.Equal(10, evolutionSlot2.DigievolutionId);
        Assert.NotNull(evolutionSlot2.DigievolutionResource);
        Assert.Equal(1, evolutionSlot2.DigievolutionResource.Level); // Não cadastrado na RAM, padrão 1

        // Validar Slots 1 e 2 (Vazios — normalizados para null)
        var slot1 = partyResource.SlotsResource[1];
        Assert.Equal(2, slot1.Index);
        Assert.Null(slot1.DigimonId);
        Assert.Null(slot1.DigimonResource);

        var slot2 = partyResource.SlotsResource[2];
        Assert.Equal(3, slot2.Index);
        Assert.Null(slot2.DigimonId);
        Assert.Null(slot2.DigimonResource);

        // Validação da Garantia Física: digimonLoader nunca foi chamado para ID 255 (sem I/O binário desnecessário)
        memoryReaderMock.Verify(m => m.ReadBytes(It.Is<int>(addr => addr != 0x00048DA4 && addr != 0x00048DA8 && addr != 0x00048DAC && addr != 0x00049878), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void TestEmptySlotId()
    {
        var addressesRepository = CreateAddressesRepository();
        var partyAddresses = addressesRepository.GetPartyAddresses();
        Assert.Equal(255, partyAddresses.EmptySlotId);
    }

    [Fact]
    public void Load_ShouldHandleNullSlotBytesGracefully()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        // Simular leitura física corrompida (null) no slot 0 (0x00048DA4)
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DA4, 4))
            .Returns((byte[]?)null);

        // Slot 1 (0x00048DA8) -> ID 2 (Monmon)
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DA8, 4))
            .Returns([2, 0, 0, 0]);

        // Slot 2 (0x00048DAC) -> ID 255 (Vazio)
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DAC, 4))
            .Returns([0xFF, 0, 0, 0]);

        // Bloco de status do Monmon (ID 2, Endereço 0x00049C54)
        var fakeMemoryBlock = new byte[1500];
        BitConverter.GetBytes((short)8).CopyTo(fakeMemoryBlock, 28); // Level 8

        memoryReaderMock.Setup(m => m.ReadBytes(0x00049C54, 1500))
            .Returns(fakeMemoryBlock);
        
        memoryReaderMock.Setup(m => m.ReadInt16(0x00049C54 - 4))
            .Returns(2); // Active Evolution ID 2

        var digievolutionSlotReader = new DigievolutionSlotReader();
        var digievolutionReader = new DigievolutionReader();
        var storedDigievolutionReader = new StoredDigievolutionReader();
        var digimonReader = new DigimonReader(memoryReaderMock.Object, digievolutionSlotReader, digievolutionReader, storedDigievolutionReader);
        var digimonSlotReader = new DigimonSlotReader(memoryReaderMock.Object);
        var partyReader = new PartyReader(digimonSlotReader);

        var digimonLoader = new DigimonLoader(addressesRepository, digimonReader);
        var partyLoader = new PartyLoader(addressesRepository, partyReader, digimonLoader);

        // 2. Act
        var partyResource = partyLoader.Load();

        // 3. Assert - Deve pular o slot 0 e o slot 2 com segurança e apenas carregar o slot 1 (Monmon)
        Assert.NotNull(partyResource);
        Assert.Equal(3, partyResource.SlotsResource.Count);

        // Slot 0 (Falha de I/O de bytes) -> Pulado
        var slot0 = partyResource.SlotsResource[0];
        Assert.Null(slot0.DigimonId);
        Assert.Null(slot0.DigimonResource);

        // Slot 1 (Monmon carregado)
        var slot1 = partyResource.SlotsResource[1];
        Assert.Equal(2, slot1.DigimonId);
        Assert.NotNull(slot1.DigimonResource);
        Assert.Equal(8, slot1.DigimonResource.Level);

        // Slot 2 (Vazio) -> Pulado
        var slot2 = partyResource.SlotsResource[2];
        Assert.Null(slot2.DigimonId);
        Assert.Null(slot2.DigimonResource);
    }

    [Fact]
    public void Load_ShouldTreatUnknownDigimonIdAsEmptySlot_WhenSlotContainsUnknownDigimonId()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DA4, 4))
            .Returns([99, 0, 0, 0]);

        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DA8, 4))
            .Returns([0xFF, 0, 0, 0]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DAC, 4))
            .Returns([0xFF, 0, 0, 0]);

        var partyLoader = CreatePartyLoader(addressesRepository, memoryReaderMock.Object);

        var partyResource = partyLoader.Load();

        Assert.NotNull(partyResource);
        Assert.Null(partyResource.SlotsResource[0].DigimonId);
        Assert.Null(partyResource.SlotsResource[0].DigimonResource);
        memoryReaderMock.Verify(m => m.ReadBytes(It.Is<int>(addr => addr != 0x00048DA4 && addr != 0x00048DA8 && addr != 0x00048DAC), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Load_ShouldHandleEmptySlotBytesGracefully()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DA4, 4))
            .Returns([]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DA8, 4))
            .Returns([0xFF, 0, 0, 0]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DAC, 4))
            .Returns([0xFF, 0, 0, 0]);

        var partyLoader = CreatePartyLoader(addressesRepository, memoryReaderMock.Object);

        var partyResource = partyLoader.Load();

        Assert.NotNull(partyResource);
        Assert.Equal(3, partyResource.SlotsResource.Count);
        Assert.Null(partyResource.SlotsResource[0].DigimonId);
        Assert.Null(partyResource.SlotsResource[0].DigimonResource);
        Assert.Null(partyResource.SlotsResource[1].DigimonId);
        Assert.Null(partyResource.SlotsResource[2].DigimonId);
        memoryReaderMock.Verify(m => m.ReadBytes(It.Is<int>(addr => addr != 0x00048DA4 && addr != 0x00048DA8 && addr != 0x00048DAC), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Load_ShouldTreatUnknownDigimonIdAsEmptySlot_WhenLaterSlotContainsUnknownDigimonId()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DA4, 4))
            .Returns([1, 0, 0, 0]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DA8, 4))
            .Returns([99, 0, 0, 0]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DAC, 4))
            .Returns([0xFF, 0, 0, 0]);

        var fakeMemoryBlock = new byte[1500];
        BitConverter.GetBytes((short)12).CopyTo(fakeMemoryBlock, 28);

        memoryReaderMock.Setup(m => m.ReadBytes(0x00049878, 1500))
            .Returns(fakeMemoryBlock);
        memoryReaderMock.Setup(m => m.ReadInt16(0x00049878 - 4))
            .Returns(5);

        var partyLoader = CreatePartyLoader(addressesRepository, memoryReaderMock.Object);

        var partyResource = partyLoader.Load();

        Assert.NotNull(partyResource);
        Assert.Equal(1, partyResource.SlotsResource[0].DigimonId);
        Assert.NotNull(partyResource.SlotsResource[0].DigimonResource);
        Assert.Null(partyResource.SlotsResource[1].DigimonId);
        Assert.Null(partyResource.SlotsResource[1].DigimonResource);
        memoryReaderMock.Verify(m => m.ReadBytes(0x00049878, 1500), Times.Once);
    }

    [Fact]
    public void Load_ShouldReturnAllEmptySlots_WhenAllSlotsAreEmpty()
    {
        var addressesRepository = CreateAddressesRepository();
        var memoryReaderMock = new Mock<IMemoryReader>();

        // Todos os 3 slots com ID 255 (0xFF)
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DA4, 4)).Returns([0xFF, 0, 0, 0]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DA8, 4)).Returns([0xFF, 0, 0, 0]);
        memoryReaderMock.Setup(m => m.ReadBytes(0x00048DAC, 4)).Returns([0xFF, 0, 0, 0]);

        var digievolutionSlotReader = new DigievolutionSlotReader();
        var digievolutionReader = new DigievolutionReader();
        var storedDigievolutionReader = new StoredDigievolutionReader();
        var digimonReader = new DigimonReader(memoryReaderMock.Object, digievolutionSlotReader, digievolutionReader, storedDigievolutionReader);
        var digimonSlotReader = new DigimonSlotReader(memoryReaderMock.Object);
        var partyReader = new PartyReader(digimonSlotReader);

        var digimonLoader = new DigimonLoader(addressesRepository, digimonReader);
        var partyLoader = new PartyLoader(addressesRepository, partyReader, digimonLoader);

        // 2. Act
        var partyResource = partyLoader.Load();

        // 3. Assert - Sucesso absoluto com slots vazios e zero I/O desnecessário
        Assert.NotNull(partyResource);
        Assert.Equal(3, partyResource.SlotsResource.Count);
        Assert.All(partyResource.SlotsResource, slot =>
        {
            Assert.Null(slot.DigimonId);
            Assert.Null(slot.DigimonResource);
        });

        // Garantir que NENHUMA tentativa de leitura de bloco de status (1500 bytes) foi feita na RAM
        memoryReaderMock.Verify(m => m.ReadBytes(It.Is<int>(addr => addr != 0x00048DA4 && addr != 0x00048DA8 && addr != 0x00048DAC), It.IsAny<int>()), Times.Never);
    }

    private static PartyLoader CreatePartyLoader(
        Backend.Memory.Repositories.IAddressesRepository addressesRepository,
        IMemoryReader memoryReader)
    {
        var digievolutionSlotReader = new DigievolutionSlotReader();
        var digievolutionReader = new DigievolutionReader();
        var storedDigievolutionReader = new StoredDigievolutionReader();
        var digimonReader = new DigimonReader(memoryReader, digievolutionSlotReader, digievolutionReader, storedDigievolutionReader);
        var digimonSlotReader = new DigimonSlotReader(memoryReader);
        var partyReader = new PartyReader(digimonSlotReader);
        var digimonLoader = new DigimonLoader(addressesRepository, digimonReader);
        return new PartyLoader(addressesRepository, partyReader, digimonLoader);
    }
}
