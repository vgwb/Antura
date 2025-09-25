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
    { text: 'About', link: '/en/about/', activeMatch: '/en/about/' },
    { text: 'Teacher Manual', link: '/en/manual/', activeMatch: '/en/manual/'},
    { text: 'Open Content', link: '/en/content/', activeMatch: '/en/content/' },
    { text: 'Developer Docs', link: '/en/dev/', activeMatch: '/en/dev/' },
    { text: 'Download', link: '/en/download'},
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
      text: 'Content',
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

function sidebarDev(): DefaultTheme.SidebarItem[] {
  return [
    {
      text: 'Developer Docs',
      items: [
        { text: 'Overview', link: '/' },
                {
          text: 'Discover Quest Design',
          link: 'quest-design/',
          collapsed: true,
          items: [
            { text: 'Quest Design', link: 'quest-design/quest-design' },
            { text: 'Quest Development', link: 'quest-design/quest-development' },
            { text: 'Quest Scripts Guidelines', link: 'quest-design/quest-scripts-guidelines' },
            { text: 'Commit message semantic', link: 'quest-design/commit-message-semantic' },
          ]
        },
        {
          text: 'Game Modules',
          link: 'game-modules/',
          collapsed: true,
          items: [
            { text: 'Project Structure', link: 'game-modules/ProjectStructure' },
            { text: 'Application Flow', link: 'game-modules/ApplicationFlow' },
            { text: 'MiniGame', link: 'game-modules/MiniGame' },
            { text: 'Teacher AI', link: 'game-modules/Teacher' },
            { text: 'Journey', link: 'game-modules/Journey' },
            { text: 'Database', link: 'game-modules/Database' },
            { text: 'Database Schemas', link: 'game-modules/DatabaseSchemas' },
            { text: 'Database Management', link: 'game-modules/DatabaseManagement' },
            { text: 'Data Flow', link: 'game-modules/DataFlow' },
            { text: 'Localization', link: 'game-modules/Localization' },
            { text: 'Arabic Rendering', link: 'game-modules/ArabicRendering' },
            { text: 'Logging', link: 'game-modules/Logging' },
            { text: 'Analytics', link: 'game-modules/Analytics' },
            { text: 'Shaders', link: 'game-modules/Shaders' },
            { text: 'Player Profile', link: 'game-modules/PlayerProfile' },
            { text: 'Antura & LivingLetters', link: 'game-modules/AnturaLivingLetters' },
            { text: 'Cat Update', link: 'game-modules/catupdate' },
          ]
        },
        {
          text: 'How to',
          link: 'how-to/',
          collapsed: true,
          items: [
            // User & Tester
            { text: 'Debug Shortcuts', link: 'how-to/DebugShortcuts' },
            { text: 'Export Player Database', link: 'how-to/ExportPlayerDatabase' },
            // Setup & Build
            { text: 'Install', link: 'how-to/INSTALL' },
            { text: 'Build', link: 'how-to/Build' },
            { text: 'Create Edition', link: 'how-to/CreateEdition' },
            // Collaboration & Guidelines
            { text: 'Collaborator', link: 'how-to/Collaborator' },
            { text: 'Developer Guidelines', link: 'how-to/DeveloperGuidelines' },
            { text: 'Refactoring Guidelines', link: 'how-to/RefactoringGuidelines' },
            // Localization & Content
            { text: 'Localization', link: 'how-to/Localization' },
            { text: 'Arabic Language', link: 'how-to/ArabicLanguage' },
            { text: 'Audio Files', link: 'how-to/AudioFiles' },
            { text: 'Fonts', link: 'how-to/Fonts' },
            { text: 'Drawings Font', link: 'how-to/DrawingsFont' },
            // Data & Tools
            { text: 'Export Google Sheet Data', link: 'how-to/ExportGoogleSheetData' },
            { text: 'Data Analysis', link: 'how-to/DataAnalysis' },
          ]
        },
      ]
    }
  ]
}

function sidebarAbout(): DefaultTheme.SidebarItem[] {
  return [
    { text: 'About Antura project', link: '/' },
    {
      text: 'History and Supporters',
      items: [
        
        { text: 'History', link: 'history' },
        { text: 'Supporters & Partners', link: 'supporters' },
        { text: 'Impact evaluation', link: 'impact' },
        { text: 'Erasmus+', link: 'erasmus' },
        { text: '🏆 Awards & Recognition', link: 'awards' },
        { text: '🌐 Open source', link: 'open-source' },
        { text: '❤️ Support us', link: 'support-us' },
      ],
    },
    {
      text: 'Credits',
      items: [
        { text: 'Credits', link: 'credits' },
        { text: 'Credits (Assets)', link: 'credits-assets' },
        { text: 'About this Website', link: 'website' },
      ],
    },
    {
      text: 'Contact',
      items: [
        { text: 'Contact & Community', link: 'contact' }
      ],
    },
    {
      text: 'License',
      items: [
        { text: 'License', link: 'license' },
        { text: 'Privacy Policy', link: '../../privacy-policy' },
      ],
    },
  ]
}

