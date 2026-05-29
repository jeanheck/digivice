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
    isConnected: true,
    groupCharisma: 0
  })
}))

const globalMocks = {
  global: {
    mocks: {
      $t: (key: string) => key
    },
    stubs: {
      Journal: true,
      AreaInformationPanel: true,
      Footer: true,
      Digimon: true
    }
  }
}

describe('App', () => {
  it('mounts renders properly', () => {
    const wrapper = mount(App, globalMocks)
    expect(wrapper.exists()).toBe(true)
  })
})
