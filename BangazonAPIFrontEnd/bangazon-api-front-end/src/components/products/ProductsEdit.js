import React, { Component } from "react"
import ProductManager from "../../modules/ProductManager"
import NavButton from "../navButton/NavButton"

class ProductsEdit extends Component {
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

    updateProduct = evt => {
        evt.preventDefault()

        const editedProduct = {
            id: parseInt(this.props.match.params.productId),
            title: this.state.title,
            description: this.state.description,
            quantity: parseInt(this.state.quantity),
            price: parseInt(this.state.price),
            productTypeId: parseInt(this.state.productTypeId),
            customerId: parseInt(this.state.customerId)
        }

        this.props.updateProducts(editedProduct)
            .then(() => this.props.history.push("/products"))
    }

    componentDidMount() {
        ProductManager.get(`${this.props.match.params.productId}`)
            .then(product => {
                this.setState({
                    title: product.title,
                    description: product.description,
                    quantity: product.quantity,
                    price: product.price,
                    productTypeId: product.productTypeId,
                    customerId: product.customerId
                })
            })
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
                                value={this.state.title}
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
                                value={this.state.description}
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
                                value={this.state.price}
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
                                value={this.state.productTypeId}
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
                                value={this.state.customerId}
                            />
                        </div>

                        <div onClick={this.updateProduct} className="submitButton">Submit</div>
                    </div>
                    <NavButton text={"Return"} history={this.props.history} path={"/products"} />
                </div>
            </React.Fragment>
        );
    }
}

export default ProductsEdit