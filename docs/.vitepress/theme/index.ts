import DefaultTheme from 'vitepress/theme'
import type { Theme } from 'vitepress'
import './custom.css'
import YouTubeVideo from './components/YouTubeVideo.vue'

export default {
  extends: DefaultTheme,
  enhanceApp({ app }) {
    app.component('YouTubeVideo', YouTubeVideo)
  }
} satisfies Theme
