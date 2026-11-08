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
    { text: 'Contenu du jeu', link: '/fr/content/', activeMatch: '/fr/content/' },
    { text: 'Developers', link: '/en/dev/', activeMatch: '/en/dev/' },
    { text: '⬇️ Télécharger', link: '/fr/download' }
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
      text: 'Projet Antura', link: 'index',
      items: [
        { text: 'Histoire', link: 'history' },
        { text: 'Crédits', link: 'credits' },
        { text: 'Soutiens et partenaires', link: 'supporters' },
        { text: 'Open source', link: 'open-source' },
        { text: 'Ressources', link: 'resources' },
        { text: 'Nous soutenir', link: 'support-us' },
        { text: 'Contact et communauté', link: 'contact' },
      ],
    },
    {
      text: 'Édition : Antura and the Letters',
      link: 'antura-and-the-letters',
      items: [
        { text: 'Évaluation de l’impact', link: 'impact' },
        { text: 'Prix et distinctions', link: 'awards' },
      ],
    },
    {
      text: 'Édition : Learn with Antura',
      link: 'learn-with-antura',
      items: [
        { text: 'Historique de conception', link: 'design-history' },
        { text: 'Erasmus+', link: 'erasmus' },
        { text: 'Ressources utilisées', link: 'credits-assets' },
      ],
    },
    {
      text: 'Site web et mentions légales',
      items: [
        { text: 'À propos de ce site', link: 'website' },
        { text: 'Licence', link: 'license' },
        { text: 'Politique de confidentialité', link: '../../privacy-policy' },
      ],
    },
  ]
}
