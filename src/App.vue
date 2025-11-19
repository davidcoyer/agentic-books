<template>
  <div class="app">
    <header class="app-header">
      <h1>Book Library</h1>
      <p class="app-subtitle">
        Simple CRUD-style list. Use this as a base for adding the "Favorite Books" feature.
      </p>
    </header>

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
  max-width: 900px;
  margin: 0 auto;
  padding: 2rem 1rem 3rem;
  font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
}

.app-header {
  margin-bottom: 1.5rem;
}

.app-header h1 {
  margin-bottom: 0.25rem;
}

.app-subtitle {
  margin: 0;
  color: #555;
  font-size: 0.95rem;
}
</style>
