import { defineAdditionalConfig, type DefaultTheme } from 'vitepress'

export default defineAdditionalConfig({
  themeConfig: {
    nav: nav(),

    docFooter: {
      prev: 'Page précédente',
      next: 'Page suivante'
    },

    outline: {
      label: 'Sur cette page'
    },

    sidebar: {
      '/fr/manual/': { base: '/fr/manual/', items: sidebarManual() },
      '/fr/content/': { base: '/fr/content/', items: sidebarContent() },
      '/fr/about/': { base: '/fr/about/', items: sidebarAbout() }
    },
  }
})

function nav(): DefaultTheme.NavItem[] {
  return [
    { text: 'À propos', link: '/fr/about/', activeMatch: '/fr/about/' },
    { text: 'Manuel', link: '/fr/manual/', activeMatch: '/fr/manual/' },
    { text: 'Contenu', link: '/fr/content/', activeMatch: '/fr/content/' },
    { text: 'Developer Docs', link: '/en/dev/', activeMatch: '/en/dev/' },
    { text: 'Télécharger', link: '/fr/download' },
    { text: 'News & Community', link: 'https://github.com/vgwb/Antura/discussions/c/news/5' },
  ]
}

function sidebarManual(): DefaultTheme.SidebarItem[] {
  return [
    {
      text: 'Teacher Manual', link: '/',
      items: [
        { text: 'Introduction', link: 'introduction' },
        { text: 'Installation', link: 'install' },
        { text: 'Configuration', link: 'setup' },
        { text: 'Guide pédagogique', link: 'classroom_guide' },
      ]
    },
    {
      text: 'Apprendre les langues',
      items: [
        { text: 'Introduction', link: 'learnlanguage_module' },
        { text: 'Comment jouer', link: 'learnlanguage_how_to_play' },
      ]
    },
    {
      text: 'Découvrir les cultures',
      items: [
        { text: 'Introduction', link: 'discover_introduction' },
        { text: 'Fonctionnalités', link: 'discover_module' },
        { text: 'Comment jouer aux quêtes', link: 'discover_how_to_play' }
      ]
    },
    { text: 'FAQ', link: 'faq' },
    { text: 'Feedback and Support', link: 'support' },
  ]
}

function sidebarContent(): DefaultTheme.SidebarItem[] {
  return [
    {
      text: 'Contenu',
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
          items: [{ text: 'Aperçu', link: '/' },
          { text: 'Quêtes', link: 'quests/' },
          { text: 'Sujets', link: 'topics/' },
          { text: 'Cartes', link: 'cards/' },
          { text: 'Mots', link: 'words/' },
          { text: 'Activités', link: 'activities/' },
          { text: 'Lieux', link: 'locations/' }
          ]
        },
      ]
    }
  ]
}

function sidebarAbout(): DefaultTheme.SidebarItem[] {
  return [
    {
      text: 'À propos du projet Antura', link: '/',
      items: [
        { text: 'Histoire', link: 'history' },
        { text: 'Soutiens & Partenaires', link: 'supporters' },
        { text: 'Évaluation d’impact', link: 'impact' },
        { text: 'Erasmus+', link: 'erasmus' },
        { text: 'Releases', link: 'releases' },
        { text: '🏆 Prix & Reconnaissances', link: 'awards' },
        { text: '🌐 Open source', link: 'open-source' },
        { text: '❤️ Nous soutenir', link: 'support-us' },
      ],
    },
    {
      text: 'Crédits',
      items: [
        { text: 'Crédits', link: 'credits' },
        { text: 'Crédits (Assets)', link: 'credits-assets' },
        { text: 'À propos du site', link: 'website' },
      ],
    },
    {
      text: 'Contact',
      items: [
        { text: 'Contact & Communauté', link: 'contact' }
      ],
    },
    {
      text: 'Licence',
      items: [
        { text: 'Licence', link: 'license' },
        { text: 'Politique de confidentialité', link: '../../privacy-policy' },
      ],
    },
  ]
}
