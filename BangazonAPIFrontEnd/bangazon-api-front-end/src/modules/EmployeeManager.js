import Settings from "./Settings"

export default {
  get(id) {
    return fetch(`${Settings.remoteURL}/employees/${id}`).then(e => e.json())
  },
  delete(id) {
    return fetch(`${Settings.remoteURL}/employees/${id}`, {
      method: "DELETE"
    }).then(e => e.json())
  },
  getAll() {
    return fetch(`${Settings.remoteURL}/employees`).then(e => e.json())
  },
  put(editedEmployee) {
    return fetch(`${Settings.remoteURL}/employees/${editedEmployee.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(editedEmployee)
    })
  },
  post(newEmployee) {
    return fetch(`${Settings.remoteURL}/employees`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(newEmployee)
    }).then(data => data.json())
  }
}