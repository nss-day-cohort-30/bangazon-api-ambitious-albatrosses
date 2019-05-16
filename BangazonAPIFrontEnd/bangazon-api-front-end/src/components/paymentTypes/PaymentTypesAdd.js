import React, { Component } from "react"
import NavButton from "../navButton/NavButton"

class PaymentTypesAdd extends Component {
    // Set initial state
    state = {
        name: "",
        accountNumber: null,
        customerId: null
    }


    handleFieldChange = evt => {
        const stateToChange = {}
        stateToChange[evt.target.id] = evt.target.value
        this.setState(stateToChange)
    }

    addPaymentType = evt => {
        evt.preventDefault()

        const newPaymentType = {
            name: this.state.name,
            accountNumber: this.state.accountNumber,
            customerId: this.state.customerId
        }

        this.props.addPaymentTypes(newPaymentType)
            .then(() => this.props.history.push("/paymentTypes"))
    }


    render() {
        return (
            <React.Fragment>
                <div className="resourceView">
                    <div className="resourceContainer">
                        <div>
                            <label htmlFor="Name" className="fieldTitle" style={{ marginTop: "4px" }}>Name:</label><br />
                            <input
                                type="text"
                                required
                                maxLength="55"
                                spellCheck="false"
                                autoComplete="off"
                                className="input"
                                style={{ width: "80%", transform: "translateY(-3px)", marginBottom: "4px" }}
                                onChange={this.handleFieldChange}
                                id="name"
                            />
                        </div>
                        <div>
                            <label htmlFor="AccountNumber" className="fieldTitle">Account Number:</label><br />
                            <input
                                type="number"
                                required
                                spellCheck="false"
                                autoComplete="off"
                                className="input"
                                style={{ transform: "translateY(-3px)" }}
                                onChange={this.handleFieldChange}
                                id="accountNumber"
                            />
                        </div>
                        <div>
                            <label htmlFor="CustomerID" className="fieldTitle">Customer ID:</label><br />
                            <input
                                type="number"
                                required
                                spellCheck="false"
                                autoComplete="off"
                                className="input"
                                style={{ transform: "translateY(-3px)" }}
                                onChange={this.handleFieldChange}
                                id="customerId"
                            />
                        </div>

                        <div onClick={this.addPaymentType} className="submitButton">Submit</div>
                    </div>
                    <NavButton text={"Return"} history={this.props.history} path={"/paymentTypes"} />
                </div>
            </React.Fragment>
        );
    }
}

export default PaymentTypesAdd