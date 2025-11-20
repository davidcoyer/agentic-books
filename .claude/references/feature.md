Feature Requirements (Summary)
1. Favorite Indicator
- Each book should display a heart icon next to it.
- Use an empty heart for “not favorited” and a filled heart for “favorited.”
- Clicking the heart toggles favorite status.
2. Data Model
Add a boolean property to each book:
{
  "id": 1,
  "title": "The Hobbit",
  "author": "J.R.R. Tolkien",
  "isFavorite": false
}

3. Persistence
- Favorite status must persist across page refreshes.
- Make an API call to mark the book as a favorite for the current user
4. Filter or Sort
Implement one of the following:
Option A: Show Favorites Only
A toggle or button that filters the list to only show favorited books.

Option B: Sort by Favorites
A control that sorts the list so favorite books appear first (favorite then alpha ordering).

5. UI Expectations
- The heart icon should be clearly visible.
- The UI should update immediately after clicking.
Avoid adding new UI libraries.
