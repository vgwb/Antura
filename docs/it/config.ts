import { defineAdditionalConfig, type DefaultTheme } from 'vitepress'

export default defineAdditionalConfig({
  themeConfig: {
    nav: nav(),

    sidebar: {
      '/it/manual/': { base: '/it/manual/', items: sidebarManual() },
      '/it/content/': { base: '/it/content/', items: sidebarContent() },
    }
  }
})

function nav(): DefaultTheme.NavItem[] {
  return [
    { text: 'Manuale', link: '/it/manual/', activeMatch: '/it/manual/' },
    { text: 'Contenuti', link: '/it/content/', activeMatch: '/it/content/' },
    { text: 'Developer', link: '/en/dev/', activeMatch: '/en/dev/' },
    { text: 'About', link: '/en/about/', activeMatch: '/en/about/' },
    { text: 'Forum', link: 'https://antura.discourse.group' }
  ]
}

function sidebarManual(): DefaultTheme.SidebarItem[] {
  return [
    { text: 'Introduzione', link: '/' },
    { text: 'Come installare e configurare', link: 'install' },
    { text: 'Imparare le lingue', link: 'learnlanguage_module' },
    {
      text: 'Discover Europe',
      items: [
        { text: 'Introduction', link: 'discover_introduction' },
        { text: 'Features', link: 'discover_module' },
        { text: 'How to play quests', link: 'discover_how_to_play' }
      ]
    },
    { text: 'Classroom Guide', link: 'classroom_guide' },
    { text: 'Feedback and Support', link: 'support' },
    { text: 'FAQ', link: 'faq' },
    { text: 'Changelog', link: 'changelog' }
  ]
}

function sidebarContent(): DefaultTheme.SidebarItem[] {
  return [
    {
      text: 'Content',
      items: [
        { text: 'Overview', link: '/' },
        { text: 'Quests', link: 'quests/' },
        { text: 'Topics', link: 'topics/' },
        { text: 'Cards', link: 'cards/' },
        { text: 'Words', link: 'words/' },
        { text: 'Activities', link: 'activities/' },
        { text: 'Locations', link: 'locations/' }
      ]
    }
  ]
}



