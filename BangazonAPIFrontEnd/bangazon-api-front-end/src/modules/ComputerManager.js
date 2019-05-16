import Settings from "./Settings"

export default {
  get(id) {
    return fetch(`${Settings.remoteURL}/computers/${id}`).then(e => e.json())
  },
  delete(id) {
    return fetch(`${Settings.remoteURL}/computers/${id}`, {
      method: "DELETE"
    }).then(e => e.json())
  },
  getAll() {
    return fetch(`${Settings.remoteURL}/computers`).then(e => e.json())
  },
  put(editedComputer) {
    return fetch(`${Settings.remoteURL}/computers/${editedComputer.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(editedComputer)
    })
  },
  post(newComputer) {
    return fetch(`${Settings.remoteURL}/computers`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(newComputer)
    }).then(data => data.json())
  }
}