import React, { Component } from "react"
import NavButton from "../navButton/NavButton"

class ProductTypesAdd extends Component {
    // Set initial state
    state = {
        name: ""
    }


    handleFieldChange = evt => {
        const stateToChange = {}
        stateToChange[evt.target.id] = evt.target.value
        this.setState(stateToChange)
    }

    addProductType = evt => {
        evt.preventDefault()

        const newProductType = {
            name: this.state.name
        }

        this.props.addProductTypes(newProductType)
            .then(() => this.props.history.push("/productTypes"))
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

                        <div onClick={this.addProductType} className="submitButton">Submit</div>
                    </div>
                    <NavButton text={"Return"} history={this.props.history} path={"/productTypes"} />
                </div>
            </React.Fragment>
        );
    }
}

export default ProductTypesAdd