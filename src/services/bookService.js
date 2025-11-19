const API_URL = 'http://localhost:5298/books'

// Generate a random user ID between 1 and 10
export const USER_ID = Math.floor(Math.random() * 10) + 1
console.log(`Current User ID: ${USER_ID}`)

const getHeaders = () => ({
  'Content-Type': 'application/json',
  'X-User-Id': USER_ID.toString()
})

export async function fetchBooks() {
  const response = await fetch(API_URL, {
    headers: {
      'X-User-Id': USER_ID.toString()
    }
  })
  if (!response.ok) {
    throw new Error('Failed to fetch books')
  }
  return await response.json()
}

export async function addBookInMemory(book) {
  const response = await fetch(API_URL, {
    method: 'POST',
    headers: getHeaders(),
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
    headers: getHeaders(),
    body: JSON.stringify(updatedBook)
  })
  if (!response.ok) {
    throw new Error('Failed to update book')
  }
}

export async function deleteBookInMemory(id) {
  const response = await fetch(`${API_URL}/${id}`, {
    method: 'DELETE',
    headers: {
      'X-User-Id': USER_ID.toString()
    }
  })
  if (!response.ok) {
    throw new Error('Failed to delete book')
  }
}
