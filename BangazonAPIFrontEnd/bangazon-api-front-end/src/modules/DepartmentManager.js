import Settings from "./Settings"

export default {
  get(id) {
    return fetch(`${Settings.remoteURL}/departments/${id}`).then(e => e.json())
  },
  delete(id) {
    return fetch(`${Settings.remoteURL}/departments/${id}`, {
      method: "DELETE"
    }).then(e => e.json())
  },
  getAll() {
    return fetch(`${Settings.remoteURL}/departments?_include=employees`).then(e => e.json())
  },
  put(editedDepartment) {
    return fetch(`${Settings.remoteURL}/departments/${editedDepartment.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(editedDepartment)
    })
  },
  post(newDepartment) {
    return fetch(`${Settings.remoteURL}/departments`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(newDepartment)
    }).then(data => data.json())
  }
}