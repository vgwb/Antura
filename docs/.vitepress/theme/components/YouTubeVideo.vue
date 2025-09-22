<script setup lang="ts">
defineProps<{
  id: string;
  title?: string;
}>();
</script>

<template>
  <div class="youtube-container">
    <iframe
      :src="`https://www.youtube.com/embed/${id}?rel=0`"
      :title="title || 'YouTube video'"
      frameborder="0"
      allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
      allowfullscreen
    ></iframe>
  </div>
</template>

<style scoped>
.youtube-container {
  position: relative;
  width: 100%;
  padding-bottom: 56.25%; /* 16:9 aspect ratio */
  height: 0;
  overflow: hidden;
  margin: 1rem 0; /* Optional: spacing */
  border-radius: 0.5rem; /* Optional: rounded corners */
}

.youtube-container iframe {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
}
</style>
