import { defineConfig } from 'vitepress'

export default defineConfig({
  title: 'Learn with Antura',
  description: 'The Learn with Antura project website and docs for Educators, Designers and Game Developers',
  cleanUrls: true,
  lastUpdated: true,
  srcDir: '.',
  // Build into docs/_production/site to align with GitHub Pages workflow
  outDir: './_production/site',
  sitemap: {
    hostname: 'https://antura.org'
  },
  head: [
    ['link', { rel: 'icon', href: '/assets/icon.png' }],
    ['meta', { name: 'author', content: 'VGWB' }],
    ['meta', { name: 'description', content: 'The Learn with Antura project website and docs for Educators, Designers and Game Developers' }],
    // Include legacy assets used in MkDocs
    ['link', { rel: 'stylesheet', href: '/assets/stylesheets/extra.css' }],
    ['link', { rel: 'stylesheet', href: '/assets/yarn/yarn-script.css' }],
    ['script', { src: '/assets/yarn/yarn-script.js', defer: '' }]
  ],
  themeConfig: {
    logo: '/assets/icon.png',
    editLink: {
      pattern: 'https://github.com/vgwb/Antura/edit/main/docs/:path',
      text: 'Edit this page on GitHub'
    },
    lastUpdated: {
      text: 'Last updated'
    },
    nav: [
      { text: 'Home', link: '/en/' },
      { text: 'Blog', link: '/en/blog/' },
      { text: 'Manual', link: '/en/manual/' },
      { text: 'Content', link: '/en/content/' },
      { text: 'About', link: '/en/about/' },
      { text: 'Forum', link: 'https://antura.discourse.group' },
    ],
    socialLinks: [
      { icon: 'github', link: 'https://github.com/vgwb/Antura' }
    ],
    footer: {
      message: '© VGWB | CC BY-NC-SA | <a href="/privacy-policy">Privacy Policy</a>',
    },
    search: {
      provider: 'local'
    },
    // Default (fallback) sidebar can be empty since we define per-section sidebars below in locales
  },
  locales: {
    root: {
      label: 'English',
      lang: 'en',
      link: '/en/',
      themeConfig: {
        nav: [
          { text: 'Home', link: '/en/' },
          { text: 'Manual', link: '/en/manual/' },
          { text: 'Content', link: '/en/content/' },
          { text: 'Developer', link: '/en/dev/' },
          { text: 'About', link: '/en/about/' },
          { text: 'Forum', link: 'https://antura.discourse.group' },
        ],
        sidebar: {
          '/en/manual/': [
            {
              text: 'Manual',
              items: [
                { text: 'Introduction', link: '/en/manual/' },
                { text: 'How to Install and Setup', link: '/en/manual/install' },
                { text: 'Learn Languages', link: '/en/manual/learnlanguage_module' },
                {
                  text: 'Discover Europe',
                  items: [
                    { text: 'Introduction', link: '/en/manual/discover_introduction' },
                    { text: 'Features', link: '/en/manual/discover_module' },
                    { text: 'How to play quests', link: '/en/manual/discover_how_to_play' },
                  ]
                },
                { text: 'Classroom Guide', link: '/en/manual/classroom_guide' },
                { text: 'Feedback and Support', link: '/en/manual/support' },
                { text: 'FAQ', link: '/en/manual/faq' },
                { text: 'Changelog', link: '/en/manual/changelog' },
              ]
            }
          ],
          '/en/content/': [
            {
              text: 'Content',
              items: [
                { text: 'Overview', link: '/en/content/' },
                { text: 'Quests', link: '/en/content/quest/' },
                { text: 'Topics', link: '/en/content/topics/' },
                { text: 'Cards', link: '/en/content/cards/' },
                { text: 'Words', link: '/en/content/words/' },
                { text: 'Activities', link: '/en/content/activities/' },
                { text: 'Locations', link: '/en/content/locations/' }
              ]
            }
          ],
          '/en/about/': [
            {
              text: 'About',
              items: [
                { text: 'Learn with Antura', link: '/en/about/' },
                { text: 'History', link: '/en/about/history' },
                { text: 'Erasmus+', link: '/en/about/erasmus/' },
                { text: 'Credits', link: '/en/about/credits' },
                { text: 'Credits (Assets)', link: '/en/about/credits-assets' },
                { text: 'License', link: '/en/about/license' },
                { text: 'Contact', link: '/about/contact' },
              ]
            }
          ],
          '/en/dev/': [
            {
              text: 'Developer Docs',
              items: [
                { text: 'Overview', link: '/en/dev/' },
                { text: 'Adventure(d) Framework', link: '/en/dev/adventured-framework/' },
                { text: 'How To', link: '/en/dev/how-to/' },
                { text: 'Language Minigames', link: '/en/dev/language-minigames/' },
                { text: 'Language Modules', link: '/en/dev/language-modules/' },
                { text: 'Quest Design', link: '/en/dev/quest-design/' }
              ]
            }
          ],
          '/blog/': [
            {
              text: 'Blog',
              items: [
                { text: 'All Posts', link: '/en/blog/' }
              ]
            }
          ],
          '/': [
            {
              text: 'Getting Started',
              items: [
                { text: 'Home', link: '/en/' },
                { text: 'Manual', link: '/en/manual/' },
                { text: 'Discover', link: '/en/discover/' },
                { text: 'About', link: '/en/about/' },
              ]
            }
          ]
        }
      }
    },
    fr: {
      label: 'Français',
      lang: 'fr',
      link: '/fr/',
      themeConfig: {
        nav: [
          { text: 'Accueil', link: '/fr/' },
          { text: 'Blog', link: '/fr/blog/' },
          { text: 'Manuel', link: '/fr/manual/' },
          { text: 'Découvrir', link: '/fr/discover/' },
          { text: 'À propos', link: '/fr/about/' },
          { text: 'Forum', link: '/forum' }
        ],
        sidebar: {
          '/fr/manual/': [
            {
              text: 'Manuel',
              items: [
                { text: 'Introduction', link: '/fr/manual/' },
                { text: 'Installation et configuration', link: '/fr/manual/install' },
                { text: 'Apprendre les langues', link: '/fr/manual/learnlanguage_module' },
                {
                  text: 'Découvrir l’Europe',
                  items: [
                    { text: 'Introduction', link: '/fr/manual/discover_introduction' },
                    { text: 'Fonctionnalités', link: '/fr/manual/discover_module' },
                    { text: 'Comment jouer aux quêtes', link: '/fr/manual/discover_how_to_play' },
                  ]
                },
                { text: 'Guide de classe', link: '/fr/manual/classroom_guide' },
                { text: 'Support', link: '/fr/manual/support' },
                { text: 'FAQ', link: '/fr/manual/faq' },
                { text: 'Journal des modifications', link: '/fr/manual/changelog' },
              ]
            }
          ],
          '/fr/discover/': [
            {
              text: 'Découvrir',
              items: [
                { text: 'Aperçu', link: '/fr/discover/' },
                { text: 'Quêtes', link: '/fr/discover/quest/' },
                { text: 'Thèmes', link: '/fr/discover/topics/' },
                { text: 'Cartes', link: '/fr/discover/cards/' },
                { text: 'Mots', link: '/fr/discover/words/' },
                { text: 'Activités', link: '/fr/discover/activities/' },
                { text: 'Lieux', link: '/fr/discover/locations/' }
              ]
            }
          ],
          '/fr/about/': [
            {
              text: 'À propos',
              items: [
                { text: 'Apprendre avec Antura', link: '/fr/about/' },
                { text: 'Histoire', link: '/fr/about/history' },
                { text: 'Erasmus+', link: '/fr/about/erasmus/' },
                { text: 'Crédits', link: '/fr/about/credits' },
                { text: 'Crédits (Ressources)', link: '/fr/about/credits-assets' },
                { text: 'Licence', link: '/fr/about/license' },
                { text: 'Contact', link: '/fr/about/contact' },
              ]
            }
          ],
          '/fr/blog/': [
            {
              text: 'Blog',
              items: [
                { text: 'Tous les articles', link: '/fr/blog/' }
              ]
            }
          ],
          '/fr/': [
            {
              text: 'Pour commencer',
              items: [
                { text: 'Accueil', link: '/fr/' },
                { text: 'Manuel', link: '/fr/manual/' },
                { text: 'Découvrir', link: '/fr/discover/' },
                { text: 'À propos', link: '/fr/about/' },
              ]
            }
          ]
        }
      }
    },
    pl: {
      label: 'Polski',
      lang: 'pl',
      link: '/pl/',
      themeConfig: {
        nav: [
          { text: 'Start', link: '/pl/' },
          { text: 'Blog', link: '/pl/blog/' },
          { text: 'Podręcznik', link: '/pl/manual/' },
          { text: 'Odkrywaj', link: '/pl/discover/' },
          { text: 'O nas', link: '/pl/about/' },
          { text: 'Forum', link: '/forum' }
        ],
        sidebar: {
          '/pl/manual/': [
            {
              text: 'Podręcznik',
              items: [
                { text: 'Wprowadzenie', link: '/pl/manual/' },
                { text: 'Instalacja i konfiguracja', link: '/pl/manual/install' },
                { text: 'Nauka języków', link: '/pl/manual/learnlanguage_module' },
                {
                  text: 'Odkrywaj Europę',
                  items: [
                    { text: 'Wprowadzenie', link: '/pl/manual/discover_introduction' },
                    { text: 'Funkcje', link: '/pl/manual/discover_module' },
                    { text: 'Jak grać w zadania', link: '/pl/manual/discover_how_to_play' },
                  ]
                },
                { text: 'Poradnik klasowy', link: '/pl/manual/classroom_guide' },
                { text: 'Wsparcie', link: '/pl/manual/support' },
                { text: 'FAQ', link: '/pl/manual/faq' },
                { text: 'Lista zmian', link: '/pl/manual/changelog' },
              ]
            }
          ],
          '/pl/content/': [
            {
              text: 'Odkrywaj',
              items: [
                { text: 'Przegląd', link: '/pl/content/' },
                { text: 'Zadania', link: '/pl/content/quest/' },
                { text: 'Tematy', link: '/pl/content/topics/' },
                { text: 'Karty', link: '/pl/content/cards/' },
                { text: 'Słowa', link: '/pl/content/words/' },
                { text: 'Aktywności', link: '/pl/content/activities/' },
                { text: 'Lokacje', link: '/pl/content/locations/' }
              ]
            }
          ],
          '/pl/about/': [
            {
              text: 'O nas',
              items: [
                { text: 'O projekcie', link: '/pl/about/' },
                { text: 'Historia', link: '/pl/about/history' },
                { text: 'Erasmus+', link: '/pl/about/erasmus/' },
                { text: 'Twórcy', link: '/pl/about/credits' },
                { text: 'Twórcy (zasoby)', link: '/pl/about/credits-assets' },
                { text: 'Licencja', link: '/pl/about/license' },
                { text: 'Kontakt', link: '/pl/about/contact' },
              ]
            }
          ],
          '/pl/blog/': [
            {
              text: 'Blog',
              items: [
                { text: 'Wszystkie wpisy', link: '/pl/blog/' }
              ]
            }
          ],
          '/pl/': [
            {
              text: 'Szybki start',
              items: [
                { text: 'Start', link: '/pl/' },
                { text: 'Podręcznik', link: '/pl/manual/' },
                { text: 'Odkrywaj', link: '/pl/discover/' },
                { text: 'O nas', link: '/pl/about/' },
              ]
            }
          ]
        }
      }
    },
  }
})
