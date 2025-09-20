import { defineConfig } from 'vitepress'

// we need this custom slugify to keep underscores in anchors (used in several IDs)
// maybe we could get rid of it if we changed all IDs to use hyphens instead of underscores
function slugifyKeepUnderscore(input: string): string {
  return input
    .normalize('NFKD')                 // split accents
    .replace(/[\u0300-\u036f]/g, '')   // remove diacritics
    .toLowerCase()
    .replace(/&/g, 'and')              // nice '&' handling
    // keep: a–z, 0–9, underscore, hyphen, and spaces (strip everything else)
    .replace(/[^a-z0-9_\-\s]/g, '')
    .trim()
    .replace(/\s+/g, '-')              // spaces → hyphens
    .replace(/-+/g, '-')               // collapse multiple hyphens
}

export default defineConfig({
  title: 'Learn with Antura',
  description: 'The Learn with Antura project website and docs for Educators, Designers and Game Developers',
  cleanUrls: true,
  lastUpdated: true,
  srcDir: '.',
  ignoreDeadLinks: true,
  sitemap: {
    hostname: 'https://antura.org'
  },
  head: [
    ['link', { rel: 'icon', type: 'image/png', href: '/icon.png' }],
    ['meta', { name: 'author', content: 'VGWB' }],
    ['meta', { property: 'og:type', content: 'website' }],
    ['meta', { property: 'og:site_name', content: 'Learn with Antura' }],
    [
      'meta',
      {
        property: 'og:image',
        content: 'https://antura.org/img/antura_gametitle.jpg'
      }
    ],
    ['meta', { property: 'og:url', content: 'https://antura.org/' }],
  ],
  themeConfig: {
    logo: '/icon.png',
    editLink: {
      pattern: 'https://github.com/vgwb/Antura/edit/main/docs/:path',
      text: 'Edit this page on GitHub'
    },
    lastUpdated: {
      text: 'Last updated'
    },
    socialLinks: [
      { icon: 'github', link: 'https://github.com/vgwb/Antura' }
    ],
    footer: {
      message: '©VGWB | CC BY-NC-SA | <a href="/privacy-policy">Privacy Policy</a>',
    },
    search: {
      provider: 'local'
    },
    outline: [2,3],
    externalLinkIcon: true,
  },
  locales: {
    root: { label: 'English', lang: 'en', link: '/en/', dir: 'ltr' },
    fr: {label: 'Français', lang: 'fr', link: '/fr/', dir: 'ltr' },
    pl: {label: 'Polski', lang: 'pl', link: '/pl/', dir: 'ltr' },
    it: {label: 'Italiano', lang: 'it', link: '/it/', dir: 'ltr' },
  },
  markdown: {
    anchor: {
      slugify: slugifyKeepUnderscore
    }
  }
})
