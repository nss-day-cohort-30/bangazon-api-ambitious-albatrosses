import Settings from "./Settings"

export default {
  get(id) {
    return fetch(`${Settings.remoteURL}/products/${id}`).then(e => e.json())
  },
  delete(id) {
    return fetch(`${Settings.remoteURL}/products/${id}`, {
      method: "DELETE"
    }).then(e => e.json())
  },
  getAll() {
    return fetch(`${Settings.remoteURL}/products`).then(e => e.json())
  },
  put(editedProduct) {
    return fetch(`${Settings.remoteURL}/products/${editedProduct.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(editedProduct)
    })
  },
  post(newProduct) {
    return fetch(`${Settings.remoteURL}/products`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(newProduct)
    }).then(data => data.json())
  }
}