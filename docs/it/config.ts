import { defineAdditionalConfig, type DefaultTheme } from 'vitepress'

export default defineAdditionalConfig({
  themeConfig: {
    nav: nav(),

    docFooter: {
      prev: 'Pagina precedente',
      next: 'Pagina successiva'
    },

    outline: {
      label: 'In questa pagina'
    },

    sidebar: {
      '/it/manual/': { base: '/it/manual/', items: sidebarManual() },
      '/it/content/': { base: '/it/content/', items: sidebarContent() },
      '/it/about/': { base: '/it/about/', items: sidebarAbout() },
    }
  }
})

function nav(): DefaultTheme.NavItem[] {
  return [
    { text: 'Informazioni', link: '/it/about/', activeMatch: '/it/about/' },
    { text: 'Manuale', link: '/it/manual/', activeMatch: '/it/manual/' },
    { text: 'Contenuti', link: '/it/content/', activeMatch: '/it/content/' },
    { text: 'Developers Docs', link: '/en/dev/', activeMatch: '/en/dev/' },
    { text: 'Download', link: '/it/download' },
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
  ]
}

function sidebarContent(): DefaultTheme.SidebarItem[] {
  return [
    {
      text: 'Contenuti',
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
            { text: 'Argomenti', link: 'topics/' },
            { text: 'Carte', link: 'cards/' },
            { text: 'Parole', link: 'words/' },
            { text: 'Attività', link: 'activities/' },
            { text: 'Luoghi', link: 'locations/' }
          ]
        },
      ]
    }
  ]
}

function sidebarAbout(): DefaultTheme.SidebarItem[] {
  return [
    { text: 'Informazioni su Antura', link: '/' },
    {
      text: 'Storia e sostenitori',
      items: [
        { text: 'Storia', link: 'history' },
        { text: 'Sostenitori & Partner', link: 'supporters' },
        { text: 'Valutazione d\'impatto', link: 'impact' },
        { text: 'Erasmus+', link: 'erasmus' },
        { text: 'Releases', link: 'releases' },
        { text: '🏆 Premi & Riconoscimenti', link: 'awards' },
        { text: '🌐 Open source', link: 'open-source' },
        { text: '❤️ Sostienici', link: 'support-us' }
      ]
    },
    {
      text: 'Crediti',
      items: [
        { text: 'Crediti', link: 'credits' },
        { text: 'Crediti (Assets)', link: 'credits-assets' },
        { text: 'Informazioni sul sito', link: 'website' }
      ]
    },
    {
      text: 'Contatti',
      items: [
        { text: 'Contatti & Community', link: 'contact' }
      ]
    },
    {
      text: 'Licenza',
      items: [
        { text: 'Licenza', link: 'license' },
        { text: 'Informativa sulla privacy', link: '../../privacy-policy' }
      ]
    }
  ]
}



