import { defineAdditionalConfig, type DefaultTheme } from 'vitepress'

export default defineAdditionalConfig({
  themeConfig: {
    nav: nav(),

    sidebar: {
      '/pl/manual/': { base: '/pl/manual/', items: sidebarManual() },
      '/pl/content/': { base: '/pl/content/', items: sidebarContent() },
    }
  }
})

function nav(): DefaultTheme.NavItem[] {
  return [
    { text: 'Podręcznik', link: '/pl/manual/', activeMatch: '/pl/manual/' },
    { text: 'Zawartość', link: '/pl/content/', activeMatch: '/pl/content/' },
    { text: 'Deweloper', link: '/en/dev/', activeMatch: '/en/dev/' },
    { text: 'O nas', link: '/en/about/', activeMatch: '/en/about/' },
    { text: 'Forum', link: 'https://antura.discourse.group' }
  ]
}

function sidebarManual(): DefaultTheme.SidebarItem[] {
  return [
    { text: 'Wprowadzenie', link: '/' },
    { text: 'Jak zainstalować i skonfigurować', link: 'install' },
    { text: 'Nauka języków', link: 'learnlanguage_module' },
    {
      text: 'Odkryj Europę',
      items: [
        { text: 'Wprowadzenie', link: 'discover_introduction' },
        { text: 'Funkcje', link: 'discover_module' },
        { text: 'Jak grać w questy', link: 'discover_how_to_play' }
      ]
    },
    { text: 'Przewodnik dla nauczycieli', link: 'classroom_guide' },
    { text: 'Opinie i wsparcie', link: 'support' },
    { text: 'FAQ', link: 'faq' },
    { text: 'Changelog', link: 'changelog' }
  ]
}

function sidebarContent(): DefaultTheme.SidebarItem[] {
  return [
    {
      text: 'Zawartość',
      items: [
        { text: 'Przegląd', link: '/' },
        { text: 'Questy', link: 'quests/' },
        { text: 'Tematy', link: 'topics/' },
        { text: 'Karty', link: 'cards/' },
        { text: 'Słowa', link: 'words/' },
        { text: 'Aktywności', link: 'activities/' },
        { text: 'Lokalizacje', link: 'locations/' }
      ]
    }
  ]
}



