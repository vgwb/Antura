import { defineAdditionalConfig, type DefaultTheme } from 'vitepress'

export default defineAdditionalConfig({
  themeConfig: {
    nav: nav(),

    docFooter: {
      prev: 'Poprzednia strona',
      next: 'Następna strona'
    },

    outline: {
      label: 'Na tej stronie'
    },

    sidebar: {
      '/pl/manual/': { base: '/pl/manual/', items: sidebarManual() },
      '/pl/content/': { base: '/pl/content/', items: sidebarContent() },
      '/pl/about/': { base: '/pl/about/', items: sidebarAbout() },
    }
  }
})

function nav(): DefaultTheme.NavItem[] {
  return [
    { text: 'O projekcie', link: '/pl/about/', activeMatch: '/pl/about/' },
    { text: 'Podręcznik', link: '/pl/manual/', activeMatch: '/pl/manual/' },
    { text: 'Zawartość gry', link: '/pl/content/', activeMatch: '/pl/content/' },
    { text: 'Developers', link: '/en/dev/', activeMatch: '/en/dev/' },
    { text: '⬇️ Pobierz', link: '/pl/download' }
  ]
}

function sidebarManual(): DefaultTheme.SidebarItem[] {
  return [
    { text: 'Teacher Manual', link: '/' ,
      items: [
        { text: 'Wprowadzenie', link: 'introduction' },
        { text: 'Instalacja', link: 'install' },
        { text: 'Konfiguracja', link: 'setup' },
        { text: 'Przewodnik dla nauczyciela', link: 'classroom_guide' },
      ]
    },
    {
      text: 'Szczegółowe moduły',
      items: [
        { text: 'Nauka języków', link: 'learnlanguage_module' },
        { text: 'Jak grać w Naukę języka', link: 'learnlanguage_how_to_play' },
      ]
    },
    {
      text: 'Discover Cultures',
      items: [
        { text: 'Odkrywaj kultury', link: 'discover_introduction' },
        { text: 'Odkrywaj kultury szczegóły', link: 'discover_module' },
        { text: 'Jak grać Odkrywaj zadania', link: 'discover_how_to_play' }
      ]
    },
    { text: 'FAQ', link: 'faq' },
    { text: 'Opinie i wsparcie', link: 'support' },
  ]
}

function sidebarContent(): DefaultTheme.SidebarItem[] {
  return [
    {
      text: 'Zawartość',
      items: [
        { text: 'Overview', link: '/' },
        {
          text: 'Learn Languages',
          items: [
            { text: 'Curriculum', link: 'language-curriculum/' },
            { text: 'Minigames', link: 'language-minigames/' },
          ]
        },
        {
          text: 'Discover Cultures',
          items: [
            { text: 'Quests', link: 'quests/' },
            { text: 'Topics', link: 'topics/' },
            { text: 'Cards', link: 'cards/' },
            { text: 'Words', link: 'words/' },
            { text: 'Activities', link: 'activities/' },
            { text: 'Locations', link: 'locations/' }
          ]
        },
      ]
    }
  ]
}

function sidebarAbout(): DefaultTheme.SidebarItem[] {
  return [
    {
      text: 'Projekt Antura', link: 'index',
      items: [
        { text: 'Historia', link: 'history' },
        { text: 'Podziękowania', link: 'credits' },
        { text: 'Wspierający i partnerzy', link: 'supporters' },
        { text: 'Open source', link: 'open-source' },
        { text: 'Zasoby', link: 'resources' },
        { text: 'Wesprzyj nas', link: 'support-us' },
        { text: 'Kontakt i społeczność', link: 'contact' },
      ]
    },
    {
      text: 'Edycja: Antura and the Letters',
      link: 'antura-and-the-letters',
      items: [
        { text: 'Ocena wpływu', link: 'impact' },
        { text: 'Nagrody i wyróżnienia', link: 'awards' },
      ]
    },
    {
      text: 'Edycja: Learn with Antura',
      link: 'learn-with-antura',
      items: [
        { text: 'Historia projektowania', link: 'design-history' },
        { text: 'Erasmus+', link: 'erasmus' },
        { text: 'Wykorzystane zasoby', link: 'credits-assets' },
      ]
    },
    {
      text: 'Strona internetowa i informacje prawne',
      items: [
        { text: 'O tej stronie', link: 'website' },
        { text: 'Licencja', link: 'license' },
        { text: 'Polityka prywatności', link: '../../privacy-policy' }
      ]
    }
  ]
}



