<template>
  <article class="book-item">
    <div class="book-main" v-if="!isEditing">
      <h3 class="book-title">{{ book.title }}</h3>
      <p class="book-author">by {{ book.author }}</p>
      <p class="book-meta">
        <span v-if="book.year">{{ book.year }}</span>
        <span v-if="book.year && book.genre"> • </span>
        <span v-if="book.genre">{{ book.genre }}</span>
        <span v-if="(book.year || book.genre) && book.pages"> • </span>
        <span v-if="book.pages">{{ book.pages }} pages</span>
      </p>
    </div>

    <div class="book-main" v-else>
      <div class="edit-row">
        <label>
          Title
          <input v-model="editTitle" type="text" />
        </label>
        <label>
          Author
          <input v-model="editAuthor" type="text" />
        </label>
      </div>
      <div class="edit-row">
        <label>
          Genre
          <input v-model="editGenre" type="text" />
        </label>
        <label>
          Year
          <input v-model="editYear" type="number" />
        </label>
        <label>
          Pages
          <input v-model="editPages" type="number" />
        </label>
      </div>
    </div>

    <div class="book-actions" v-if="!isEditing">
      <!-- This is where the favorite/star UI will go in the exercise -->
      <button type="button" class="edit-button" @click="startEdit">
        Edit
      </button>
      <button type="button" class="delete-button" @click="$emit('delete')">
        Delete
      </button>
    </div>

    <div class="book-actions" v-else>
      <button type="button" class="save-button" @click="saveEdit">
        Save
      </button>
      <button type="button" class="cancel-button" @click="cancelEdit">
        Cancel
      </button>
    </div>
  </article>
</template>

<script setup>
import { ref, watch } from 'vue'

const props = defineProps({
  book: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['delete', 'update'])

const isEditing = ref(false)
const editTitle = ref('')
const editAuthor = ref('')
const editGenre = ref('')
const editYear = ref('')
const editPages = ref('')

watch(
  () => props.book,
  (newBook) => {
    if (!isEditing.value && newBook) {
      editTitle.value = newBook.title
      editAuthor.value = newBook.author
      editGenre.value = newBook.genre || ''
      editYear.value = newBook.year ?? ''
      editPages.value = newBook.pages ?? ''
    }
  },
  { immediate: true }
)

function startEdit() {
  isEditing.value = true
}

function cancelEdit() {
  isEditing.value = false
  editTitle.value = props.book.title
  editAuthor.value = props.book.author
  editGenre.value = props.book.genre || ''
  editYear.value = props.book.year ?? ''
  editPages.value = props.book.pages ?? ''
}

function saveEdit() {
  const parsedYear = editYear.value ? parseInt(editYear.value, 10) : null
  const parsedPages = editPages.value ? parseInt(editPages.value, 10) : null

  const updated = {
    ...props.book,
    title: editTitle.value.trim(),
    author: editAuthor.value.trim(),
    genre: editGenre.value.trim() || null,
    year: Number.isNaN(parsedYear) ? null : parsedYear,
    pages: Number.isNaN(parsedPages) ? null : parsedPages
  }

  emit('update', updated)
  isEditing.value = false
}
</script>

<style scoped>
.book-item {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 0.75rem;
  padding: 0.6rem 0.75rem;
  border-radius: 6px;
  background-color: #fff;
  border: 1px solid #e0e0e0;
}

.book-main {
  flex: 1;
  min-width: 0;
}

.book-title {
  margin: 0;
  font-size: 1rem;
}

.book-author {
  margin: 0.1rem 0 0;
  font-size: 0.9rem;
  color: #666;
}

.book-meta {
  margin: 0.15rem 0 0;
  font-size: 0.85rem;
  color: #777;
}

.book-actions {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.edit-row {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  margin-top: 0.3rem;
}

.edit-row label {
  display: flex;
  flex-direction: column;
  font-size: 0.8rem;
  min-width: 120px;
}

.edit-row input {
  margin-top: 0.15rem;
  padding: 0.25rem 0.35rem;
  border-radius: 4px;
  border: 1px solid #ccc;
}

.edit-button,
.delete-button,
.save-button,
.cancel-button {
  padding: 0.25rem 0.6rem;
  border-radius: 4px;
  font-size: 0.8rem;
  cursor: pointer;
  border: 1px solid transparent;
}

.edit-button {
  border-color: #888;
  background-color: #f5f5f5;
}

.delete-button {
  border-color: #cc0000;
  background-color: #cc0000;
  color: white;
}

.save-button {
  border-color: #0077cc;
  background-color: #0077cc;
  color: white;
}

.cancel-button {
  border-color: #888;
  background-color: #f5f5f5;
}
</style>
