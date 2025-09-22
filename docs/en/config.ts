import { defineAdditionalConfig, type DefaultTheme } from 'vitepress'

export default defineAdditionalConfig({
  themeConfig: {
    nav: nav(),

    sidebar: {
      '/en/manual/': { base: '/en/manual/', items: sidebarManual() },
      '/en/content/': { base: '/en/content/', items: sidebarContent() },
      '/en/dev/': { base: '/en/dev/', items: sidebarDev() },
      '/en/about/': { base: '/en/about/', items: sidebarAbout() }
    }
  }
})

function nav(): DefaultTheme.NavItem[] {
  return [
    { text: 'Manual', link: '/en/manual/', activeMatch: '/en/manual/'},
    { text: 'Content', link: '/en/content/', activeMatch: '/en/content/' },
    { text: 'Developer', link: '/en/dev/', activeMatch: '/en/dev/' },
    { text: 'About', link: '/en/about/', activeMatch: '/en/about/' },
    { text: 'Download', link: '/en/download'},
    { text: 'Forum', link: 'https://antura.discourse.group' }
  ]
}

function sidebarManual(): DefaultTheme.SidebarItem[] {
  return [
    { text: 'User Manual', link: '/' ,
      items: [
        { text: 'How to Install and Setup', link: 'install' },
        { text: 'How to play Learn with Antura', link: 'how_to_play' }
      ]
    },
    {
      text: 'Learn Languages Modules',
      items: [
        { text: 'Learn Languages overview', link: 'learnlanguage_module' },
        { text: 'How to play minigames', link: 'learnlanguage_how_to_play' }
      ]
    },
    {
      text: 'Discover Modules',
      items: [
        { text: 'Discover overview', link: 'discover_introduction' },
        { text: 'Discover features', link: 'discover_module' },
        { text: 'How to play quests', link: 'discover_how_to_play' }
      ]
    },
    {
      text: 'In the Classroom',
      items: [
        { text: 'Classroom Guide', link: 'classroom_guide' },
        { text: 'Feedback and Support', link: 'support' },
      ]
    },
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

// function autoSidebar(slug: string): DefaultTheme.SidebarItem[] {
//   // Grab all .md files under this slug using Vite's glob (typed via vite/client)
//   const modules = import.meta.glob(`/en/dev/${slug}/*.md`, { eager: true }) as Record<string, any>

//   const pages = Object.entries(modules)
//     .map(([file, mod]) => {
//       const name = file.split('/').pop()?.replace(/\.md$/, '') || ''
//       if (!name || name === 'index') return null
//       const fm = (mod as any)?.frontmatter ?? {}
//       const text = (fm.title as string) || name.replace(/-/g, ' ')
//       const order = typeof fm.order === 'number' ? (fm.order as number) : 1e9
//       const link = `${slug}/${name}/`
//       return { text, link, order }
//     })
//     .filter(Boolean) as { text: string; link: string; order: number }[]

//   pages.sort((a, b) => a.order - b.order || a.text.localeCompare(b.text))
//   return pages.map(({ text, link }) => ({ text, link }))
// }

function sidebarDev(): DefaultTheme.SidebarItem[] {
  return [
    {
      text: 'Developer Docs',
      items: [
        { text: 'Overview', link: '/' },
        { text: 'AdventurED Framework', link: 'adventured-framework/' },
        // { text: 'How To', collapsed: false, items: autoSidebar('how-to') },
        // {
        //   text: 'Language Minigames',
        //   collapsed: false,
        //   items: autoSidebar('language-minigames')
        // },
        { text: 'Language Modules', link: 'language-modules/' },
        { text: 'Quest Design', link: 'quest-design/' }
      ]
    }
  ]
}

function sidebarAbout(): DefaultTheme.SidebarItem[] {
  return [
    {
      text: 'About',
      items: [
        { text: 'About Antura', link: '/' },
        { text: 'History', link: 'history' },
        { text: 'Erasmus+', link: 'erasmus/' },
        { text: 'Credits', link: 'credits' },
        { text: 'Credits (Assets)', link: 'credits-assets' },
        { text: 'License', link: 'license' },
        { text: 'Contact', link: '/about/contact' }
      ]
    }
  ]
}

