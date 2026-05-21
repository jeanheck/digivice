import { describe, it, expect, vi } from 'vitest'
import { mount } from '@vue/test-utils'
import App from '../App.vue'

vi.mock('../stores/use-game-store', () => ({
  useGameStore: () => ({
    currentState: {
      player: { name: 'Agumon', bits: 123 },
      party: { slots: [null, null, null] },
      journal: { mainQuest: null, sideQuests: [] }
    },
    areaInformation: { location: { name: 'File Island' }, enemies: [] },
    isConnected: true
  })
}))

vi.mock('../composables/useLocalization', () => ({
  useLocalization: () => ({
    getLocalizedQuest: (q: any) => q
  })
}))

const globalMocks = {
  global: {
    mocks: {
      $t: (key: string) => key
    },
    stubs: {
      QuestJournalPanel: true,
      AreaInformationPanel: true,
      QuestDetailsModal: true,
      PlayerFooter: true,
      DigimonCard: true
    }
  }
}

describe('App', () => {
  it('mounts renders properly', () => {
    const wrapper = mount(App, globalMocks)
    expect(wrapper.exists()).toBe(true)
  })
})
