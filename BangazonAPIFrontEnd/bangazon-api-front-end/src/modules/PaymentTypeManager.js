import Settings from "./Settings"

export default {
  get(id) {
    return fetch(`${Settings.remoteURL}/paymentType/${id}`).then(e => e.json())
  },
  delete(id) {
    return fetch(`${Settings.remoteURL}/paymentType/${id}`, {
      method: "DELETE"
    }).then(e => e.json())
  },
  getAll() {
    return fetch(`${Settings.remoteURL}/paymentType`).then(e => e.json())
  },
  put(editedPaymentType) {
    return fetch(`${Settings.remoteURL}/paymentType/${editedPaymentType.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(editedPaymentType)
    })
  },
  post(newPaymentType) {
    return fetch(`${Settings.remoteURL}/paymentType`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(newPaymentType)
    }).then(data => data.json())
  }
}