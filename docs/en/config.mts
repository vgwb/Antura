import { defineAdditionalConfig, type DefaultTheme } from 'vitepress'

export default defineAdditionalConfig({
  themeConfig: {
    nav: nav(),

    sidebar: {
      '/en/manual/': { base: '/en/manual/', items: sidebarManual() },
      '/en/content/': { base: '/en/content/', items: sidebarContent() },
      '/en/dev/': { base: '/en/dev/', items: sidebarDev() },
      '/en/about/': { base: '/en/about/', items: sidebarAbout() },
      '/en/news/': { base: '/en/news/', items: sidebarNews() }
    }
  }
})

function nav(): DefaultTheme.NavItem[] {
  return [
    { text: 'About', link: '/en/about/', activeMatch: '/en/about/' },
    { text: 'Manual', link: '/en/manual/', activeMatch: '/en/manual/' },
    { text: 'Game Content', link: '/en/content/', activeMatch: '/en/content/' },
    { text: 'News', link: '/en/news/', activeMatch: '/en/news/' },
    { text: 'Developers', link: '/en/dev/', activeMatch: '/en/dev/' },
    { text: '⬇️ Download', link: '/en/download' }
  ]
}

function sidebarManual(): DefaultTheme.SidebarItem[] {
  return [
    {
      text: 'Teacher Manual', link: '/',
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
          ]
        },
        {
          text: 'How to',
          link: 'how-to/',
          collapsed: true,
          items: [
            // Setup & Build
            { text: 'Install', link: 'how-to/INSTALL' },
            { text: 'Commit message semantic', link: 'how-to/CommitMessageSemantic' },
            { text: 'Build from sources', link: 'how-to/Build' },
            // User & Tester
            { text: 'Debug Shortcuts', link: 'how-to/DebugShortcuts' },
            { text: 'Export Player Database', link: 'how-to/ExportPlayerDatabase' },
            // Collaboration & Guidelines
            { text: 'Collaborator', link: 'how-to/Collaborator' },
            { text: 'Developer Guidelines', link: 'how-to/DeveloperGuidelines' },
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
    {
      text: 'About Antura Project',
      link: 'index',
      items: [
        { text: 'History', link: 'history' },
        { text: 'Credits', link: 'credits' },
        { text: 'Supporters & Partners', link: 'supporters' },
        { text: '🌐 Open source', link: 'open-source' },
        { text: 'Resources', link: 'resources' },
        { text: '❤️ Support us', link: 'support-us' },
        { text: 'Contact & Community', link: 'contact' },
      ],
    },
    {
      text: 'Edition: Antura and the Letters',
      link: 'antura-and-the-letters',
      items: [
        { text: 'Impact evaluation', link: 'impact' },
        { text: '🏆 Awards & Recognition', link: 'awards' },
      ],
    },
    {
      text: 'Edition: Learn with Antura',
      link: 'learn-with-antura',
      items: [
        { text: 'Design History', link: 'design-history' },
        { text: 'Erasmus+', link: 'erasmus' },
        { text: 'Assets used', link: 'credits-assets' },
     ],
    },
    {
      text: 'Website & Legal',
      items: [
        { text: 'About this Website', link: 'website' },
        { text: 'License', link: 'license' },
        { text: 'Privacy Policy', link: '../../privacy-policy' },
      ],
    },
  ]
}

function sidebarNews(): DefaultTheme.SidebarItem[] {
  return [
    {
      text: 'News',
      link: '/',
      items: [
        {
          text: 'Project news',
          items: [
            { text: 'Fifth Consortium Meeting in Wrocław', link: 'posts/2026-05-fifth-consortium-meeting-wroclaw' },
            { text: 'Teacher capacity-building trainings', link: 'posts/2026-04-capacity-building-trainings' },
            { text: 'Language-teaching seminar', link: 'posts/2026-02-language-teaching-seminar' },
            { text: 'Game Studies Congress in Kraków', link: 'posts/2025-11-game-studies-congress-krakow' },
            { text: 'Large-scale pilot in France and Poland', link: 'posts/2025-10-large-scale-pilot-france-poland' },
            { text: 'Fourth Consortium Meeting in Wrocław', link: 'posts/2025-09-fourth-consortium-meeting-wroclaw' },
            { text: 'Small-scale pilot begins in France', link: 'posts/2025-03-small-scale-pilot-france' },
            { text: 'Third Consortium Meeting in Málaga', link: 'posts/2024-11-third-consortium-meeting-malaga' },
            { text: 'Consortium Meeting in Angers', link: 'posts/2024-07-consortium-meeting' },
            { text: 'Workshops in Wrocław', link: 'posts/2024-03-workshop-warklav' },
            { text: 'Workshops in Angers', link: 'posts/2024-02-workshop-angers' },
            { text: 'Kick off meeting - Cologne', link: 'posts/2023-12-kickoff-meeting' },
          ],
        },
        {
          text: 'Awards & Recognition',
          items: [
            { text: 'German Developer Award 2022', link: 'posts/2022-12-german-developer-award' },
            { text: 'UNHCR NGO Innovation Award', link: 'posts/2019-12-unhcr-innovation-award-antura' },
            { text: 'Spanish Red Cross Technology Award', link: 'posts/2019-02-spanish-red-cross-award' },
            { text: 'TechFugees 2018', link: 'posts/2018-11-techfugees-antura' },
            { text: 'Games for Change 2018 finalists', link: 'posts/2018-06-games-for-change-finalist' },
            { text: 'Titanium Serious Game of the Year', link: 'posts/2017-12-titanium-serious-game-award' },
            { text: 'EduApp4Syria finalist', link: 'posts/2016-09-eduapp4syria-finalists' },
          ],
        },
        {
          text: 'Early Antura Coverage',
          items: [
            { text: 'International Mother Language Day', link: 'posts/2018-02-mother-language-day-antura' },
            { text: 'January 2018 project update', link: 'posts/2018-02-antura-january-newsletter' },
            { text: 'Education 2030 forum', link: 'posts/2017-10-antura-education-2030' },
            { text: 'International Literacy Day', link: 'posts/2017-09-international-literacy-day' },
            { text: 'Downloads, testing and open source', link: 'posts/2017-09-antura-project-update' },
            { text: 'FMX talks published', link: 'posts/2017-07-fmx-talks-published' },
            { text: 'FMX 2017 presentation', link: 'posts/2017-05-antura-at-fmx' },
            { text: 'April 2017 project update', link: 'posts/2017-05-antura-april-newsletter' },
            { text: 'When We Play, We Learn', link: 'posts/2017-03-when-we-play-we-learn' },
            { text: 'First game images', link: 'posts/2016-08-antura-first-images' },
          ],
        },
        {
          text: 'Releases',
          items: [
            { text: '2026 beta', link: 'posts/2026-11-angers' },
            { text: '2025 Discover Edition beta', link: 'posts/2025-01-discover-edition-beta' },
            { text: '2.4.0 - Pashto', link: 'posts/2022-01-v2-4-pashto' },
            { text: '2.3.0 - Hungarian', link: 'posts/2022-01-v2-3-hungarian' },
            { text: '2.2.0 - New Languages', link: 'posts/2022-07-v2-2-new-languages' },
            { text: '2.1.0 - Multiedition One App', link: 'posts/2022-06-v2-1-multiedition' },
            { text: '2.0.0 - New Arabic', link: 'posts/2021-11-v2-0-new-arabic' },
            { text: '1.4.0 - Learn French', link: 'posts/2020-08-v1-4-learn-french' },
            { text: '1.3.0 - Learn English', link: 'posts/2019-02-v1-3-learn-english' },
            { text: '1.2.0 - Arabic Release', link: 'posts/2018-02-v1-2-arabic-release' },
            { text: '0.6.0 - Arabic Beta', link: 'posts/2017-03-v0-6-arabic-beta' },
          ],
        },
      ],
    },
  ]
}

