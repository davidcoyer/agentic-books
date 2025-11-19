<template>
  <section class="book-list">
    <div class="book-list-header">
      <h2>Books</h2>
    </div>

    <div class="book-list-content">
      <div class="toolbar">
        <label class="toolbar-search">
          <span>Search:</span>
          <input
            v-model="searchTerm"
            type="text"
            placeholder="Filter by title, author, or genre"
          />
        </label>
      </div>

      <div v-if="isLoading" class="status-text">
        Loading books...
      </div>
      <div v-else-if="filteredBooks.length === 0" class="status-text">
        No books found.
      </div>
      <ul v-else class="book-items">
        <li
          v-for="book in filteredBooks"
          :key="book.id"
          class="book-item-wrapper"
        >
          <BookItem
            :book="book"
            @delete="handleDelete(book.id)"
            @update="handleUpdate"
          />
        </li>
      </ul>
    </div>

    <section class="add-book">
      <h3>Add a New Book</h3>
      <form @submit.prevent="handleAdd">
        <div class="form-row">
          <label>
            Title
            <input v-model="newTitle" type="text" placeholder="e.g. The Hobbit" />
          </label>
          <label>
            Author
            <input v-model="newAuthor" type="text" placeholder="e.g. J.R.R. Tolkien" />
          </label>
        </div>

        <div class="form-row">
          <label>
            Genre
            <input v-model="newGenre" type="text" placeholder="e.g. Fantasy" />
          </label>
          <label>
            Year
            <input v-model="newYear" type="number" placeholder="e.g. 1937" />
          </label>
          <label>
            Pages
            <input v-model="newPages" type="number" placeholder="e.g. 310" />
          </label>
        </div>

        <button type="submit" :disabled="!canAdd">
          Add Book
        </button>
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
  deleteBookInMemory
} from '../services/bookService'

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
    // isFavorite: will be added in the exercise
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
.book-list {
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 1.5rem;
  background-color: #fafafa;
}

.book-list-header {
  margin-bottom: 1rem;
}

.book-list-content {
  margin-bottom: 1.5rem;
}

.toolbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.75rem;
  gap: 1rem;
  flex-wrap: wrap;
}

.toolbar-search {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.toolbar-search input {
  padding: 0.3rem 0.5rem;
  border-radius: 4px;
  border: 1px solid #ccc;
}

.status-text {
  font-size: 0.95rem;
  color: #666;
  padding: 0.5rem 0;
}

.book-items {
  list-style: none;
  padding: 0;
  margin: 0;
}

.book-item-wrapper + .book-item-wrapper {
  margin-top: 0.5rem;
}

.add-book {
  border-top: 1px solid #e0e0e0;
  padding-top: 1rem;
  margin-top: 1rem;
}

.add-book h3 {
  margin-top: 0;
}

.form-row {
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
  margin-bottom: 0.75rem;
}

.form-row label {
  flex: 1 1 160px;
  display: flex;
  flex-direction: column;
  font-size: 0.9rem;
}

.form-row input {
  margin-top: 0.25rem;
  padding: 0.35rem 0.5rem;
  border-radius: 4px;
  border: 1px solid #ccc;
}

button[type='submit'] {
  padding: 0.4rem 0.9rem;
  border-radius: 4px;
  border: 1px solid #0077cc;
  background-color: #0077cc;
  color: white;
  font-size: 0.95rem;
  cursor: pointer;
}

button[disabled] {
  opacity: 0.6;
  cursor: not-allowed;
}
</style>
