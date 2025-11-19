const API_URL = 'http://localhost:5298/books'

export async function fetchBooks() {
  const response = await fetch(API_URL)
  if (!response.ok) {
    throw new Error('Failed to fetch books')
  }
  return await response.json()
}

export async function addBookInMemory(book) {
  const response = await fetch(API_URL, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(book)
  })
  if (!response.ok) {
    throw new Error('Failed to add book')
  }
  return await response.json()
}

export async function updateBookInMemory(updatedBook) {
  const response = await fetch(`${API_URL}/${updatedBook.id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(updatedBook)
  })
  if (!response.ok) {
    throw new Error('Failed to update book')
  }
}

export async function deleteBookInMemory(id) {
  const response = await fetch(`${API_URL}/${id}`, {
    method: 'DELETE'
  })
  if (!response.ok) {
    throw new Error('Failed to delete book')
  }
}
