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
    { text: 'Zawartość', link: '/pl/content/', activeMatch: '/pl/content/' },
    { text: 'Deweloper', link: '/en/dev/', activeMatch: '/en/dev/' },
    { text: 'Download', link: '/pl/download' },
    { text: 'News & Community', link: 'https://antura.discourse.group/c/news/5' }
  ]
}

function sidebarManual(): DefaultTheme.SidebarItem[] {
  return [
    { text: 'Teacher Manual', link: '/' ,
      items: [
        { text: 'Introduction', link: 'introduction' },
        { text: 'Install', link: 'install' },
        { text: 'Setup', link: 'setup' },
        { text: 'Classroom Guide', link: 'classroom_guide' },
      ]
    },
    {
      text: 'Learn Languages',
      items: [
        { text: 'Learn Languages overview', link: 'learnlanguage_module' },
        { text: 'How to play minigames', link: 'learnlanguage_how_to_play' },
      ]
    },
    {
      text: 'Discover Cultures',
      items: [
        { text: 'Discover overview', link: 'discover_introduction' },
        { text: 'Discover features', link: 'discover_module' },
        { text: 'How to play quests', link: 'discover_how_to_play' }
      ]
    },
    { text: 'FAQ', link: 'faq' },
    { text: 'Feedback and Support', link: 'support' },
    { text: 'Changelog', link: 'changelog' }
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
    { text: 'O projekcie Antura', link: '/' },
    {
      text: 'Historia i partnerzy',
      items: [
        { text: 'Historia', link: 'history' },
        { text: 'Wspierający i partnerzy', link: 'supporters' },
        { text: 'Ocena wpływu', link: 'impact' },
        { text: 'Erasmus+', link: 'erasmus' },
        { text: '🏆 Nagrody i wyróżnienia', link: 'awards' },
        { text: '🌐 Open source', link: 'open-source' },
        { text: '❤️ Wsparcie', link: 'support-us' }
      ]
    },
    {
      text: 'Zasługi',
      items: [
        { text: 'Zasługi', link: 'credits' },
        { text: 'Zasługi (Assets)', link: 'credits-assets' },
        { text: 'O stronie', link: 'website' }
      ]
    },
    {
      text: 'Kontakt',
      items: [
        { text: 'Kontakt i społeczność', link: 'contact' }
      ]
    },
    {
      text: 'Licencja',
      items: [
        { text: 'Licencja', link: 'license' },
        { text: 'Polityka prywatności', link: '../../privacy-policy' }
      ]
    }
  ]
}



