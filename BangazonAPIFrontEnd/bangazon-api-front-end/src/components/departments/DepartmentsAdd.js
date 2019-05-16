import React, { Component } from "react"
import NavButton from "../navButton/NavButton"

class DepartmentsAdd extends Component {
    // Set initial state
    state = {
        name: "",
        budget: 0
    }


    handleFieldChange = evt => {
        const stateToChange = {}
        stateToChange[evt.target.id] = evt.target.value
        this.setState(stateToChange)
    }

    addDepartment = evt => {
        evt.preventDefault()

        const newDepartment = {
            name: this.state.name,
            budget: this.state.budget
        }

        this.props.addDepartments(newDepartment)
            .then(() => this.props.history.push("/departments"))
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
                                style={{ transform: "translateY(-3px)", marginBottom: "4px" }}
                                onChange={this.handleFieldChange}
                                id="name"
                            />
                        </div>
                        <div>
                            <label htmlFor="Budget" className="fieldTitle" >Budget:</label><br />
                            <input
                                type="number"
                                required
                                spellCheck="false"
                                autoComplete="off"
                                className="input"
                                style={{ transform: "translateY(-3px)" }}
                                onChange={this.handleFieldChange}
                                id="budget"
                            />
                        </div>

                        <div onClick={this.addDepartment} className="submitButton">Submit</div>
                    </div>
                    <NavButton text={"Return"} history={this.props.history} path={"/departments"} />
                </div>
            </React.Fragment>
        );
    }
}

export default DepartmentsAdd