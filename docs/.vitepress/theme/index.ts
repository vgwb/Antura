import { h, nextTick, watch } from "vue";
import type { Theme } from "vitepress";
import DefaultTheme from "vitepress/theme";
import { useData } from "vitepress";
import { createMermaidRenderer } from "vitepress-mermaid-renderer";

import "./custom.css";
import YouTubeVideo from "./components/YouTubeVideo.vue";

export default {
  extends: DefaultTheme,
  enhanceApp({ app }) {
    app.component("YouTubeVideo", YouTubeVideo);
  },
  Layout: () => {
    const { isDark } = useData();

    const initMermaid = () => {
      const mermaidRenderer = createMermaidRenderer({
        theme: isDark.value ? "dark" : "forest",
      });

      // Disable the interactive toolbar entirely.
      mermaidRenderer.setToolbar({
        showLanguageLabel: false,
        desktop: {
          zoomIn: "enabled",
          zoomOut: "enabled",
          resetView: "enabled",
          copyCode: "disabled",
          toggleFullscreen: "enabled",
          download: "disabled",
          zoomLevel: "disabled",
        },
        mobile: {
          zoomIn: "disabled",
          zoomOut: "disabled",
          resetView: "disabled",
          copyCode: "disabled",
          toggleFullscreen: "disabled",
          download: "disabled",
          zoomLevel: "disabled",
        },
        fullscreen: {
          zoomIn: "disabled",
          zoomOut: "disabled",
          resetView: "disabled",
          copyCode: "disabled",
          toggleFullscreen: "disabled",
          download: "disabled",
          zoomLevel: "disabled",
        },
      });
    };

    // initial mermaid setup
    nextTick(() => initMermaid());

    // on theme change, re-render mermaid charts
    watch(
      () => isDark.value,
      () => {
        initMermaid();
      },
    );

    return h(DefaultTheme.Layout);
  },
} satisfies Theme;
