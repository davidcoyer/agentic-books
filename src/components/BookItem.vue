<template>
  <article class="book-card" v-if="!isEditing">
    <div class="book-content">
      <h3 class="book-title">{{ book.title }}</h3>
      <p class="book-author">by {{ book.author }}</p>
      <div class="book-meta">
        <span class="genre-badge">{{ book.genre }}</span>
        <span class="meta-dot">•</span>
        <span>{{ book.year }}</span>
        <span class="meta-dot">•</span>
        <span>{{ book.pages }} pages</span>
      </div>
    </div>
    <div class="book-actions">
      <button type="button" class="action-btn edit-btn" @click="startEdit">Edit</button>
      <button type="button" class="action-btn delete-btn" @click="$emit('delete')">Delete</button>
    </div>
  </article>

  <article class="book-card edit-mode" v-else>
    <div class="edit-form">
      <div class="form-row">
        <div class="form-field">
          <label>Title</label>
          <input v-model="editTitle" type="text" placeholder="e.g. The Hobbit" />
        </div>
        <div class="form-field">
          <label>Author</label>
          <input v-model="editAuthor" type="text" placeholder="e.g. J.R.R. Tolkien" />
        </div>
      </div>
      <div class="form-row">
        <div class="form-field">
          <label>Genre</label>
          <input v-model="editGenre" type="text" placeholder="e.g. Fantasy" />
        </div>
        <div class="form-field">
          <label>Year</label>
          <input v-model="editYear" type="number" placeholder="2025" />
        </div>
        <div class="form-field">
          <label>Pages</label>
          <input v-model="editPages" type="number" placeholder="0" />
        </div>
      </div>
    </div>
    <div class="book-actions">
      <button type="button" class="action-btn save-btn" @click="saveEdit">Save</button>
      <button type="button" class="action-btn cancel-btn" @click="cancelEdit">Cancel</button>
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
.book-card {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.25rem 1.5rem;
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  transition: box-shadow 0.2s;
}

.book-card:hover {
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
}

.book-content {
  flex: 1;
  min-width: 0;
}

.book-title {
  margin: 0 0 0.25rem 0;
  font-size: 1.125rem;
  font-weight: 600;
  color: #1f2937;
}

.book-author {
  margin: 0 0 0.5rem 0;
  font-size: 0.875rem;
  color: #6b7280;
}

.book-meta {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.875rem;
  color: #6b7280;
}

.genre-badge {
  display: inline-block;
  padding: 0.25rem 0.625rem;
  background: #eff6ff;
  color: #1e40af;
  border-radius: 4px;
  font-size: 0.75rem;
  font-weight: 500;
}

.meta-dot {
  color: #d1d5db;
}

.book-actions {
  display: flex;
  gap: 0.5rem;
  margin-left: 1rem;
}

.action-btn {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 6px;
  font-size: 0.875rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
}

.edit-btn {
  background: #f3f4f6;
  color: #374151;
}

.edit-btn:hover {
  background: #e5e7eb;
}

.delete-btn {
  background: #fee2e2;
  color: #dc2626;
}

.delete-btn:hover {
  background: #fecaca;
}

.save-btn {
  background: #1f2937;
  color: white;
}

.save-btn:hover {
  background: #111827;
}

.cancel-btn {
  background: #f3f4f6;
  color: #374151;
}

.cancel-btn:hover {
  background: #e5e7eb;
}

/* Edit mode styles */
.edit-mode {
  flex-direction: column;
  align-items: stretch;
  gap: 1rem;
}

.edit-form {
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
  padding: 0.5rem 0.75rem;
  border: 1px solid #d1d5db;
  border-radius: 6px;
  font-size: 0.875rem;
  transition: border-color 0.2s;
}

.form-field input:focus {
  outline: none;
  border-color: #3b82f6;
}

.edit-mode .book-actions {
  justify-content: flex-end;
  margin-left: 0;
}
</style>
