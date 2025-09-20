import { defineAdditionalConfig, type DefaultTheme } from 'vitepress'

export default defineAdditionalConfig({
    description: 'Antura French.',

    themeConfig: {
        nav: nav(),

        sidebar: {
            '/fr/manual/': { base: '/fr/manual/', items: sidebarManual() },
            '/fr/content/': { base: '/fr/content/', items: sidebarContent() },
        },
    }
})

function nav(): DefaultTheme.NavItem[] {
    return [
        { text: 'Manuel', link: '/fr/manual/', activeMatch: '/fr/manual/' },
        { text: 'Contenu', link: '/fr/content/', activeMatch: '/fr/content/' },
        { text: 'Développeur', link: '/en/dev/', activeMatch: '/en/dev/' },
        { text: 'À propos', link: '/en/about/', activeMatch: '/en/about/' },
        { text: 'Forum', link: 'https://antura.discourse.group' },
    ]
}

function sidebarManual(): DefaultTheme.SidebarItem[] {
    return [
        { text: 'Introduction', link: '/' },
        { text: 'Installation et configuration', link: '/install' },
        { text: 'Apprendre les langues', link: '/learnlanguage_module' },
        {
            text: 'Découvrir l\'Europe',
            items: [
                { text: 'Introduction', link: '/discover_introduction' },
                { text: 'Fonctionnalités', link: '/discover_module' },
                { text: 'Comment jouer aux quêtes', link: '/discover_how_to_play' },
            ]
        },
        { text: 'Guide de classe', link: '/classroom_guide' },
        { text: 'Feedback and Support', link: '/support' },
        { text: 'FAQ', link: '/faq' },
        { text: 'Changelog', link: '/changelog' },
    ]
}

function sidebarContent(): DefaultTheme.SidebarItem[] {
    return [
        {
            text: 'Contenu',
            items: [
                { text: 'Aperçu', link: '/' },
                { text: 'Quêtes', link: '/quests/' },
                { text: 'Sujets', link: '/topics/' },
                { text: 'Cartes', link: '/cards/' },
                { text: 'Mots', link: '/words/' },
                { text: 'Activités', link: '/activities/' },
                { text: 'Lieux', link: '/locations/' }
            ]
        }
    ]
}

