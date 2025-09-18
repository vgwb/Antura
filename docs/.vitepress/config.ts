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
      { text: 'Home', link: '/' },
      { text: 'Blog', link: '/blog/' },
      { text: 'Manual', link: '/manual/' },
      { text: 'Discover', link: '/discover/' },
      { text: 'About', link: '/about/' },
      { text: 'Forum', link: '/forum' },
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
      themeConfig: {
        nav: [
          { text: 'Home', link: '/' },
          { text: 'Blog', link: '/blog/' },
          { text: 'Manual', link: '/manual/' },
          { text: 'Discover', link: '/discover/' },
          { text: 'About', link: '/about/' },
          { text: 'Forum', link: '/forum' },
        ],
        sidebar: {
          '/manual/': [
            {
              text: 'Manual',
              items: [
                { text: 'Introduction', link: '/manual/' },
                { text: 'How to Install and Setup', link: '/manual/install' },
                { text: 'Learn Languages', link: '/manual/learnlanguage_module' },
                {
                  text: 'Discover Europe',
                  items: [
                    { text: 'Introduction', link: '/manual/discover_introduction' },
                    { text: 'Features', link: '/manual/discover_module' },
                    { text: 'How to play quests', link: '/manual/discover_how_to_play' },
                  ]
                },
                { text: 'Classroom Guide', link: '/manual/classroom_guide' },
                { text: 'Feedback and Support', link: '/manual/support' },
                { text: 'FAQ', link: '/manual/faq' },
                { text: 'Changelog', link: '/manual/changelog' },
              ]
            }
          ],
          '/discover/': [
            {
              text: 'Discover',
              items: [
                { text: 'Overview', link: '/discover/' },
                { text: 'Quests', link: '/discover/quest/' },
                { text: 'Topics', link: '/discover/topics/' },
                { text: 'Cards', link: '/discover/cards/' },
                { text: 'Words', link: '/discover/words/' },
                { text: 'Activities', link: '/discover/activities/' },
                { text: 'Locations', link: '/discover/locations/' }
              ]
            }
          ],
          '/about/': [
            {
              text: 'About',
              items: [
                { text: 'Learn with Antura', link: '/about/' },
                { text: 'History', link: '/about/history' },
                { text: 'Erasmus+', link: '/about/erasmus/' },
                { text: 'Credits', link: '/about/credits' },
                { text: 'Credits (Assets)', link: '/about/credits-assets' },
                { text: 'License', link: '/about/license' },
                { text: 'Contact', link: '/about/contact' },
              ]
            }
          ],
          '/dev/': [
            {
              text: 'Developer Docs',
              items: [
                { text: 'Overview', link: '/dev/' },
                { text: 'Adventure(d) Framework', link: '/dev/adventured-framework/' },
                { text: 'How To', link: '/dev/how-to/' },
                { text: 'Language Minigames', link: '/dev/language-minigames/' },
                { text: 'Language Modules', link: '/dev/language-modules/' },
                { text: 'Quest Design', link: '/dev/quest-design/' }
              ]
            }
          ],
          '/blog/': [
            {
              text: 'Blog',
              items: [
                { text: 'All Posts', link: '/blog/' }
              ]
            }
          ],
          '/': [
            {
              text: 'Getting Started',
              items: [
                { text: 'Home', link: '/' },
                { text: 'Manual', link: '/manual/' },
                { text: 'Discover', link: '/discover/' },
                { text: 'About', link: '/about/' },
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
          '/pl/discover/': [
            {
              text: 'Odkrywaj',
              items: [
                { text: 'Przegląd', link: '/pl/discover/' },
                { text: 'Zadania', link: '/pl/discover/quest/' },
                { text: 'Tematy', link: '/pl/discover/topics/' },
                { text: 'Karty', link: '/pl/discover/cards/' },
                { text: 'Słowa', link: '/pl/discover/words/' },
                { text: 'Aktywności', link: '/pl/discover/activities/' },
                { text: 'Lokacje', link: '/pl/discover/locations/' }
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
    ar: {
      label: 'العربية',
      lang: 'ar',
      dir: 'rtl',
      link: '/ar/',
      themeConfig: {
        nav: [
          { text: 'الرئيسية', link: '/ar/' },
          { text: 'المدونة', link: '/ar/blog/' },
          { text: 'الدليل', link: '/ar/manual/' },
          { text: 'اكتشف', link: '/ar/discover/' },
          { text: 'عن', link: '/ar/about/' },
          { text: 'المنتدى', link: '/forum' }
        ],
        sidebar: {
          '/ar/manual/': [
            {
              text: 'الدليل',
              items: [
                { text: 'مقدمة', link: '/ar/manual/' },
                { text: 'التثبيت والإعداد', link: '/ar/manual/install' },
                { text: 'تعلّم اللغات', link: '/ar/manual/learnlanguage_module' },
                {
                  text: 'اكتشف أوروبا',
                  items: [
                    { text: 'مقدمة', link: '/ar/manual/discover_introduction' },
                    { text: 'الميزات', link: '/ar/manual/discover_module' },
                    { text: 'كيف تلعب المهام', link: '/ar/manual/discover_how_to_play' },
                  ]
                },
                { text: 'دليل الفصول', link: '/ar/manual/classroom_guide' },
                { text: 'الدعم والتغذية الراجعة', link: '/ar/manual/support' },
                { text: 'الأسئلة الشائعة', link: '/ar/manual/faq' },
                { text: 'سجل التغييرات', link: '/ar/manual/changelog' },
              ]
            }
          ],
          '/ar/discover/': [
            {
              text: 'اكتشف',
              items: [
                { text: 'نظرة عامة', link: '/ar/discover/' },
                { text: 'المهام', link: '/ar/discover/quest/' },
                { text: 'المواضيع', link: '/ar/discover/topics/' },
                { text: 'البطاقات', link: '/ar/discover/cards/' },
                { text: 'الكلمات', link: '/ar/discover/words/' },
                { text: 'الأنشطة', link: '/ar/discover/activities/' },
                { text: 'الأماكن', link: '/ar/discover/locations/' }
              ]
            }
          ],
          '/ar/about/': [
            {
              text: 'عن',
              items: [
                { text: 'عن المشروع', link: '/ar/about/' },
                { text: 'التاريخ', link: '/ar/about/history' },
                { text: 'إيراسموس+', link: '/ar/about/erasmus/' },
                { text: 'الشكر والتقدير', link: '/ar/about/credits' },
                { text: 'الشكر (الأصول)', link: '/ar/about/credits-assets' },
                { text: 'الرخصة', link: '/ar/about/license' },
                { text: 'اتصال', link: '/ar/about/contact' },
              ]
            }
          ],
          '/ar/blog/': [
            {
              text: 'المدونة',
              items: [
                { text: 'كل المقالات', link: '/ar/blog/' }
              ]
            }
          ],
          '/ar/': [
            {
              text: 'ابدأ',
              items: [
                { text: 'الرئيسية', link: '/ar/' },
                { text: 'الدليل', link: '/ar/manual/' },
                { text: 'اكتشف', link: '/ar/discover/' },
                { text: 'عن', link: '/ar/about/' },
              ]
            }
          ]
        }
      }
    }
  }
})
