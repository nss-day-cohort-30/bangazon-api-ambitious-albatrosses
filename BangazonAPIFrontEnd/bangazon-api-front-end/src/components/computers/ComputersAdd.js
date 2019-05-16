import React, { Component } from "react"
import NavButton from "../navButton/NavButton"

class ComputersAdd extends Component {
    // Set initial state
    state = {
        make: "",
        manufacturer: "",
        purchaseDate: ""
    }


    handleFieldChange = evt => {
        const stateToChange = {}
        stateToChange[evt.target.id] = evt.target.value
        this.setState(stateToChange)
    }

    addComputer = evt => {
        evt.preventDefault()

        const newComputer = {
            make: this.state.make,
            manufacturer: this.state.manufacturer,
            purchaseDate: this.state.purchaseDate
        }

        this.props.addComputers(newComputer)
            .then(() => this.props.history.push("/computers"))
    }

    render() {
        return (
            <React.Fragment>
                <div className="resourceView">
                    <div className="resourceContainer">
                        <div>
                            <label htmlFor="Brand" className="fieldTitle" style={{ marginTop: "4px" }}>Brand:</label><br />
                            <input
                                type="text"
                                required
                                maxLength="55"
                                spellCheck="false"
                                autoComplete="off"
                                className="input"
                                style={{ transform: "translateY(-3px)", marginBottom: "4px" }}
                                onChange={this.handleFieldChange}
                                id="manufacturer"
                            />
                        </div>
                        <div>
                            <label htmlFor="Model" className="fieldTitle" style={{ marginTop: "4px" }}>Model:</label><br />
                            <input
                                type="text"
                                required
                                maxLength="55"
                                spellCheck="false"
                                autoComplete="off"
                                className="input"
                                style={{ transform: "translateY(-3px)", marginBottom: "4px" }}
                                onChange={this.handleFieldChange}
                                id="make"
                            />
                        </div>
                        <div>
                            <label htmlFor="PurchaseDate" className="fieldTitle">Purchase Date:</label><br />
                            <input
                                type="date"
                                required
                                spellCheck="false"
                                autoComplete="off"
                                className="input"
                                style={{ transform: "translateY(-3px)" }}
                                onChange={this.handleFieldChange}
                                id="purchaseDate"
                            />
                        </div>

                        <div onClick={this.addComputer} className="submitButton">Submit</div>
                    </div>
                    <NavButton text={"Return"} history={this.props.history} path={"/computers"} />
                </div>
            </React.Fragment>
        );
    }
}

export default ComputersAdd