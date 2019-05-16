import React, { Component } from 'react'
import NavButton from "../navButton/NavButton"


class ProductsView extends Component {

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

    formatNumber(num) {
        return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
    }

    render() {
        return (
            <React.Fragment>
                <div className="resourceView">
                    <div className="messageText">Products</div>
                    <input type="text" spellCheck="false" autoComplete="off" className="input" onChange={this.handleNameFieldChange} placeholder="Search name"></input>
                    <input type="number" min="1" spellCheck="false" autoComplete="off" className="input" onChange={this.handleIdFieldChange} placeholder="Search ID"></input>
                    <div className="resourceContainer">
                        {
                            this.props.products
                                .filter(p => p.title.toUpperCase().includes(this.state.nameSearchTerms.toUpperCase()))
                                .filter(p => (this.state.idSearchTerms !== null) ? p.id === this.state.idSearchTerms : p)
                                .map(product => {
                                    return (
                                        <div key={product.id} className="resource">
                                            <div><span className="darkerText">ID: </span><strong>{product.id}</strong></div>
                                            <div><span className="darkerText">Title: </span><strong>{product.title}</strong></div>
                                            <div><span className="darkerText">Description: </span><br /><strong>{product.description}</strong></div>
                                            <div><span className="darkerText">Price: </span>${this.formatNumber(parseInt(product.price))}</div>
                                            <div><span className="darkerText">Quantity: </span>{product.quantity}</div>
                                            <div className="editButton" onClick={() => this.props.history.push("/products/" + product.id + "/edit")}>Edit</div>
                                        </div>
                                    )
                                })
                        }
                    </div>
                    <NavButton text={"Return"} history={this.props.history} path={"/"} />
                    <NavButton text={"Add New Product"} history={this.props.history} path={"/products/add"} />
                </div>
            </React.Fragment>
        )
    }
}

export default ProductsView