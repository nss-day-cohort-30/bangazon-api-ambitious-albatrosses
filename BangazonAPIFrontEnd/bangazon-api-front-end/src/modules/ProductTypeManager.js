import Settings from "./Settings"

export default {
  get(id) {
    return fetch(`${Settings.remoteURL}/productTypes/${id}`).then(e => e.json())
  },
  delete(id) {
    return fetch(`${Settings.remoteURL}/productTypes/${id}`, {
      method: "DELETE"
    }).then(e => e.json())
  },
  getAll() {
    return fetch(`${Settings.remoteURL}/productTypes`).then(e => e.json())
  },
  put(editedProductType) {
    return fetch(`${Settings.remoteURL}/productTypes/${editedProductType.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(editedProductType)
    })
  },
  post(newProductType) {
    return fetch(`${Settings.remoteURL}/productTypes`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(newProductType)
    }).then(data => data.json())
  }
}