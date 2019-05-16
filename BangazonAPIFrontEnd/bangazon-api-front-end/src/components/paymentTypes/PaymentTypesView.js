import React, { Component } from 'react'
import NavButton from "../navButton/NavButton"


class PaymentTypesView extends Component {

    state = {
        nameSearchTerms: "",
        idSearchTerms: null
    }

    handleNameFieldChange = evt => {
        const stateToChange = {}
        stateToChange.nameSearchTerms = evt.target.value
        this.setState(stateToChange)
    }

    handleIdFieldChange = evt => {
        const stateToChange = {}
        stateToChange.idSearchTerms = parseInt(evt.target.value)
        if (evt.target.value === "") {
            stateToChange.idSearchTerms = null;
        }
        this.setState(stateToChange)
    }

    render() {
        return (
            <React.Fragment>
                <div className="resourceView">
                    <div className="messageText">Payment Types</div>
                    <input type="text" spellCheck="false" autoComplete="off" className="input" onChange={this.handleNameFieldChange} placeholder="Search name"></input>
                    <input type="number" min="1" spellCheck="false" autoComplete="off" className="input" onChange={this.handleIdFieldChange} placeholder="Search ID"></input>
                    <div className="resourceContainer">
                        {
                            this.props.paymentTypes
                                .filter(p => p.name.toUpperCase().includes(this.state.nameSearchTerms.toUpperCase()))
                                .filter(p => (this.state.idSearchTerms !== null) ? p.id === this.state.idSearchTerms : p)
                                .map(payment => {
                                    return (
                                        <div key={payment.id} className="resource">
                                            <div><span className="darkerText">ID: </span><strong>{payment.id}</strong></div>
                                            <div><span className="darkerText">Name: </span><strong>{payment.name}</strong></div>
                                            <div><span className="darkerText">Account Number: </span><strong>{payment.accountNumber}</strong></div>
                                            <div><span className="darkerText">Customer ID: </span><strong>{payment.customerId}</strong></div>
                                            <div className="editButton" onClick={() => this.props.history.push("/paymentTypes/" + payment.id + "/edit")}>Edit</div>
                                        </div>
                                    )
                                })
                        }
                    </div>
                    <NavButton text={"Return"} history={this.props.history} path={"/"} />
                    <NavButton text={"Add New Payment Type"} history={this.props.history} path={"/paymentTypes/add"} />
                </div>
            </React.Fragment>
        )
    }
}

export default PaymentTypesView