import React, { Component } from "react"
import NavButton from "../navButton/NavButton"

class ProductsAdd extends Component {
    // Set initial state
    state = {
        title: "",
        description: "",
        quantity: 0,
        price: 0,
        productTypeId: 0,
        customerId: 0,
    }


    handleFieldChange = evt => {
        const stateToChange = {}
        stateToChange[evt.target.id] = evt.target.value
        this.setState(stateToChange)
    }

    addProduct = evt => {
        evt.preventDefault()

        const newProduct = {
            title: this.state.title,
            description: this.state.description,
            quantity: parseInt(this.state.quantity),
            price: parseInt(this.state.price),
            productTypeId: parseInt(this.state.productTypeId),
            customerId: parseInt(this.state.customerId)
        }

        this.props.addProducts(newProduct)
            .then(() => this.props.history.push("/products"))
    }

    render() {
        return (
            <React.Fragment>
                <div className="resourceView">
                    <div className="resourceContainer">
                        <div>
                            <label htmlFor="Title" className="fieldTitle" style={{ marginTop: "4px" }}>Title:</label><br />
                            <input
                                type="text"
                                required
                                maxLength="255"
                                spellCheck="false"
                                autoComplete="off"
                                className="input"
                                style={{ width: "80%", transform: "translateY(-3px)", marginBottom: "4px" }}
                                onChange={this.handleFieldChange}
                                id="title"
                            />
                        </div>
                        <div>
                            <label htmlFor="Description" className="fieldTitle" style={{ marginTop: "4px" }}>Description:</label><br />
                            <input
                                type="text"
                                required
                                maxLength="255"
                                spellCheck="false"
                                autoComplete="off"
                                className="input"
                                style={{ width: "80%", transform: "translateY(-3px)", marginBottom: "4px" }}
                                onChange={this.handleFieldChange}
                                id="description"
                            />
                        </div>
                        <div>
                            <label htmlFor="Price" className="fieldTitle">Price:</label><br />
                            <input
                                type="number"
                                required
                                spellCheck="false"
                                autoComplete="off"
                                className="input"
                                style={{ transform: "translateY(-3px)" }}
                                onChange={this.handleFieldChange}
                                id="price"
                            />
                        </div>
                        <div>
                            <label htmlFor="ProductTypeId" className="fieldTitle">Product Type ID:</label><br />
                            <input
                                type="number"
                                required
                                min="1"
                                spellCheck="false"
                                autoComplete="off"
                                className="input"
                                style={{ transform: "translateY(-3px)" }}
                                onChange={this.handleFieldChange}
                                id="productTypeId"
                            />
                        </div>
                        <div>
                            <label htmlFor="CustomerId" className="fieldTitle">Customer ID:</label><br />
                            <input
                                type="number"
                                required
                                min="1"
                                spellCheck="false"
                                autoComplete="off"
                                className="input"
                                style={{ transform: "translateY(-3px)" }}
                                onChange={this.handleFieldChange}
                                id="customerId"
                            />
                        </div>

                        <div onClick={this.addProduct} className="submitButton">Submit</div>
                    </div>
                    <NavButton text={"Return"} history={this.props.history} path={"/products"} />
                </div>
            </React.Fragment>
        );
    }
}

export default ProductsAdd