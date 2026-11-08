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
    { text: 'Contenuti del gioco', link: '/it/content/', activeMatch: '/it/content/' },
    { text: 'Developers', link: '/en/dev/', activeMatch: '/en/dev/' },
    { text: '⬇️ Scarica', link: '/it/download' }
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
    {
      text: 'Progetto Antura', link: 'index',
      items: [
        { text: 'Storia', link: 'history' },
        { text: 'Crediti', link: 'credits' },
        { text: 'Sostenitori e partner', link: 'supporters' },
        { text: 'Open source', link: 'open-source' },
        { text: 'Risorse', link: 'resources' },
        { text: 'Sostienici', link: 'support-us' },
        { text: 'Contatti e comunità', link: 'contact' },
      ]
    },
    {
      text: 'Edizione: Antura and the Letters',
      link: 'antura-and-the-letters',
      items: [
        { text: 'Valutazione dell’impatto', link: 'impact' },
        { text: 'Premi e riconoscimenti', link: 'awards' },
      ]
    },
    {
      text: 'Edizione: Learn with Antura',
      link: 'learn-with-antura',
      items: [
        { text: 'Storia del design', link: 'design-history' },
        { text: 'Erasmus+', link: 'erasmus' },
        { text: 'Risorse utilizzate', link: 'credits-assets' },
      ]
    },
    {
      text: 'Sito web e informazioni legali',
      items: [
        { text: 'Informazioni su questo sito', link: 'website' },
        { text: 'Licenza', link: 'license' },
        { text: 'Informativa sulla privacy', link: '../../privacy-policy' }
      ]
    }
  ]
}



