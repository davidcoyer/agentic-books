<template>
  <div class="app">
    <main>
      <BookList />
    </main>
  </div>
</template>

<script setup>
import BookList from './components/BookList.vue'
</script>

<style scoped>
.app {
  font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
}
</style>
