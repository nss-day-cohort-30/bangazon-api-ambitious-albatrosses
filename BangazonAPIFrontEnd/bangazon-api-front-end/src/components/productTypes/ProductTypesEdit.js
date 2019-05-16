import React, { Component } from "react"
import ProductTypeManager from "../../modules/ProductTypeManager"
import NavButton from "../navButton/NavButton"

class ProductTypesEdit extends Component {
    // Set initial state
    state = {
        name: ""
    }


    handleFieldChange = evt => {
        const stateToChange = {}
        stateToChange[evt.target.id] = evt.target.value
        this.setState(stateToChange)
    }

    updateProductType = evt => {
        evt.preventDefault()

        const editedProductType = {
            id: parseInt(this.props.match.params.productTypeId),
            name: this.state.name
        }

        this.props.updateProductTypes(editedProductType)
            .then(() => this.props.history.push("/productTypes"))
    }

    componentDidMount() {
        ProductTypeManager.get(`${this.props.match.params.productTypeId}`)
            .then(productType => {
                this.setState({
                    name: productType.name
                })
            })
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
                                value={this.state.name}
                            />
                        </div>

                        <div onClick={this.updateProductType} className="submitButton">Submit</div>
                    </div>
                    <NavButton text={"Return"} history={this.props.history} path={"/productTypes"} />
                </div>
            </React.Fragment>
        );
    }
}

export default ProductTypesEdit