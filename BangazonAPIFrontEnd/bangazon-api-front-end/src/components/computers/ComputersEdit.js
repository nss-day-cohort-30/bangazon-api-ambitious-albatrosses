import React, { Component } from "react"
import ComputerManager from "../../modules/ComputerManager"
import NavButton from "../navButton/NavButton"

class ComputersEdit extends Component {
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

    updateComputer = evt => {
        evt.preventDefault()

        const editedComputer = {
            id: parseInt(this.props.match.params.computerId),
            make: this.state.make,
            manufacturer: this.state.manufacturer,
            purchaseDate: this.state.purchaseDate
        }

        this.props.updateComputers(editedComputer)
            .then(() => this.props.history.push("/computers"))
    }

    componentDidMount() {
        ComputerManager.get(`${this.props.match.params.computerId}`)
            .then(computer => {
                this.setState({
                    make: computer.make,
                    manufacturer: computer.manufacturer,
                    purchaseDate: computer.purchaseDate
                })
            })
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
                                value={this.state.manufacturer}
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
                                value={this.state.make}
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
                                value={this.state.purchaseDate}
                            />
                        </div>

                        <div onClick={this.updateComputer} className="submitButton">Submit</div>
                    </div>
                    <NavButton text={"Return"} history={this.props.history} path={"/computers"} />
                </div>
            </React.Fragment>
        );
    }
}

export default ComputersEdit