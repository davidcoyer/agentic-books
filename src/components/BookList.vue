<template>
  <section class="book-library">
    <!-- Header -->
    <div class="library-header">
      <div class="header-content">
        <div class="header-title">
          <div class="icon-wrapper">
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M4 19.5v-15A2.5 2.5 0 0 1 6.5 2H20v20H6.5a2.5 2.5 0 0 1 0-5H20"></path>
            </svg>
          </div>
          <h1>Book Library</h1>
        </div>
        <div class="user-badge">User ID: {{ userId }}</div>
      </div>
    </div>

    <!-- Search and Filter -->
    <div class="controls-bar">
      <div class="search-box">
        <svg class="search-icon" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <circle cx="11" cy="11" r="8"></circle>
          <path d="m21 21-4.3-4.3"></path>
        </svg>
        <input
          v-model="searchTerm"
          type="text"
          placeholder="Filter by title or author..."
          class="search-input"
        />
      </div>
      <div class="filter-group">
        <span class="filter-label">Show:</span>
        <button class="filter-btn active">All Books</button>
      </div>
    </div>

    <!-- Book List -->
    <div class="books-container">
      <div v-if="isLoading" class="status-message">
        Loading books...
      </div>
      <div v-else-if="filteredBooks.length === 0" class="status-message">
        No books found.
      </div>
      <div v-else class="books-list">
        <BookItem
          v-for="book in filteredBooks"
          :key="book.id"
          :book="book"
          @delete="handleDelete(book.id)"
          @update="handleUpdate"
        />
      </div>
    </div>

    <!-- Add Book Form -->
    <section class="add-book-section">
      <h2 class="section-title">Add a New Book</h2>
      <form @submit.prevent="handleAdd" class="add-book-form">
        <div class="form-row">
          <div class="form-field">
            <label>Title</label>
            <input v-model="newTitle" type="text" placeholder="e.g. The Hobbit" />
          </div>
          <div class="form-field">
            <label>Author</label>
            <input v-model="newAuthor" type="text" placeholder="e.g. J.R.R. Tolkien" />
          </div>
        </div>

        <div class="form-row">
          <div class="form-field">
            <label>Genre</label>
            <input v-model="newGenre" type="text" placeholder="e.g. Fantasy" />
          </div>
          <div class="form-field">
            <label>Year</label>
            <input v-model="newYear" type="number" placeholder="2025" />
          </div>
          <div class="form-field">
            <label>Pages</label>
            <input v-model="newPages" type="number" placeholder="0" />
          </div>
        </div>

        <div class="form-actions">
          <button type="submit" :disabled="!canAdd" class="add-btn">
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M5 12h14"></path>
              <path d="M12 5v14"></path>
            </svg>
            Add Book
          </button>
        </div>
      </form>
    </section>
  </section>
</template>

<script setup>
import { computed, onMounted, ref } from 'vue'
import BookItem from './BookItem.vue'
import {
  fetchBooks,
  addBookInMemory,
  updateBookInMemory,
  deleteBookInMemory,
  USER_ID
} from '../services/bookService'

const userId = USER_ID
const books = ref([])
const isLoading = ref(true)
const searchTerm = ref('')

const newTitle = ref('')
const newAuthor = ref('')
const newGenre = ref('')
const newYear = ref('')
const newPages = ref('')

onMounted(async () => {
  isLoading.value = true
  try {
    const initialBooks = await fetchBooks()
    books.value = initialBooks
  } finally {
    isLoading.value = false
  }
})

const filteredBooks = computed(() => {
  const term = searchTerm.value.trim().toLowerCase()
  if (!term) {
    return books.value
  }
  return books.value.filter((book) => {
    return (
      book.title.toLowerCase().includes(term) ||
      book.author.toLowerCase().includes(term) ||
      (book.genre && book.genre.toLowerCase().includes(term))
    )
  })
})

const canAdd = computed(() => {
  return newTitle.value.trim().length > 0 && newAuthor.value.trim().length > 0
})

function handleAdd() {
  if (!canAdd.value) return

  const nextId = books.value.length
    ? Math.max(...books.value.map((b) => b.id)) + 1
    : 1

  const parsedYear = newYear.value ? parseInt(newYear.value, 10) : null
  const parsedPages = newPages.value ? parseInt(newPages.value, 10) : null

  const newBook = {
    id: nextId,
    title: newTitle.value.trim(),
    author: newAuthor.value.trim(),
    genre: newGenre.value.trim() || null,
    year: Number.isNaN(parsedYear) ? null : parsedYear,
    pages: Number.isNaN(parsedPages) ? null : parsedPages
  }

  books.value.push(newBook)
  addBookInMemory(newBook)

  newTitle.value = ''
  newAuthor.value = ''
  newGenre.value = ''
  newYear.value = ''
  newPages.value = ''
}

function handleDelete(id) {
  books.value = books.value.filter((book) => book.id !== id)
  deleteBookInMemory(id)
}

function handleUpdate(updatedBook) {
  const index = books.value.findIndex((b) => b.id === updatedBook.id)
  if (index !== -1) {
    books.value[index] = { ...updatedBook }
  }
  updateBookInMemory(updatedBook)
}
</script>

<style scoped>
.book-library {
  max-width: 56rem;
  margin: 0 auto;
  padding: 2rem 1rem;
}

/* Header */
.library-header {
  margin-bottom: 2rem;
}

.header-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.5rem;
}

.header-title {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.icon-wrapper {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 2.5rem;
  height: 2.5rem;
  background: #1f2937;
  border-radius: 8px;
  color: white;
}

.library-header h1 {
  margin: 0;
  font-size: 1.875rem;
  font-weight: 700;
  color: #1f2937;
}


.user-badge {
  background-color: #f3f4f6;
  padding: 0.375rem 0.875rem;
  border-radius: 12px;
  font-size: 0.875rem;
  font-weight: 600;
  color: #374151;
}

/* Controls */
.controls-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  margin-bottom: 1.5rem;
  padding: 1rem;
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
}

.search-box {
  position: relative;
  flex: 1;
  max-width: 24rem;
}

.search-icon {
  position: absolute;
  left: 0.75rem;
  top: 50%;
  transform: translateY(-50%);
  color: #9ca3af;
}

.search-input {
  width: 100%;
  padding: 0.625rem 0.75rem 0.625rem 2.5rem;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  font-size: 0.875rem;
  transition: border-color 0.2s;
}

.search-input:focus {
  outline: none;
  border-color: #3b82f6;
}

.filter-group {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.filter-label {
  font-size: 0.875rem;
  color: #6b7280;
  font-weight: 500;
}

.filter-btn {
  padding: 0.5rem 1rem;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  background: white;
  font-size: 0.875rem;
  font-weight: 500;
  color: #374151;
  cursor: pointer;
  transition: all 0.2s;
}

.filter-btn.active {
  background: #1f2937;
  color: white;
  border-color: #1f2937;
}

.filter-btn:hover:not(.active) {
  background: #f9fafb;
}

/* Books Container */
.books-container {
  margin-bottom: 2rem;
}

.status-message {
  padding: 2rem;
  text-align: center;
  color: #6b7280;
  font-size: 0.875rem;
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
}

.books-list {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

/* Add Book Section */
.add-book-section {
  padding: 1.5rem;
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
}

.section-title {
  margin: 0 0 1.25rem 0;
  font-size: 1.25rem;
  font-weight: 600;
  color: #1f2937;
}

.add-book-form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.form-row {
  display: flex;
  gap: 1rem;
}

.form-field {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 0.375rem;
}

.form-field label {
  font-size: 0.875rem;
  font-weight: 500;
  color: #374151;
}

.form-field input {
  padding: 0.625rem 0.75rem;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  font-size: 0.875rem;
  transition: border-color 0.2s;
}

.form-field input:focus {
  outline: none;
  border-color: #3b82f6;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  margin-top: 0.5rem;
}

.add-btn {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.625rem 1.25rem;
  background: #1f2937;
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 0.875rem;
  font-weight: 500;
  cursor: pointer;
  transition: background 0.2s;
}

.add-btn:hover:not(:disabled) {
  background: #111827;
}

.add-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>
