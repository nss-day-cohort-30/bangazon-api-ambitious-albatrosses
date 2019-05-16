import React, { Component } from "react"
import NavButton from "../navButton/NavButton"

class EmployeesAdd extends Component {
    // Set initial state
    state = {
        firstName: "",
        lastName: "",
        departmentId: 1,
        isSuperVisor: false
    }


    handleFieldChange = evt => {
        const stateToChange = {}
        stateToChange[evt.target.id] = evt.target.value
        this.setState(stateToChange)
    }

    handleBoolChange = evt => {
        const stateToChange = {}
        if (evt.target.value === "true"){
            stateToChange[evt.target.id] = true
        }
        if (evt.target.value === "false"){
            stateToChange[evt.target.id] = false
        }
        this.setState(stateToChange)
    }

    addEmployee = evt => {
        evt.preventDefault()

        const newEmployee = {
            firstName: this.state.firstName,
            lastName: this.state.lastName,
            departmentId: this.state.departmentId,
            isSuperVisor: this.state.isSuperVisor
        }

        this.props.addEmployees(newEmployee)
            .then(() => this.props.history.push("/employees"))
    }


    render() {
        return (
            <React.Fragment>
                <div className="resourceView">
                    <div className="resourceContainer">
                        <div>
                            <label htmlFor="FirstName" className="fieldTitle" style={{ marginTop: "4px" }}>First Name:</label><br />
                            <input
                                type="text"
                                required
                                maxLength="55"
                                spellCheck="false"
                                autoComplete="off"
                                className="input"
                                style={{ transform: "translateY(-3px)", marginBottom: "4px" }}
                                onChange={this.handleFieldChange}
                                id="firstName"
                                value={this.state.firstName}
                            />
                        </div>
                        <div>
                            <label htmlFor="LastName" className="fieldTitle">Last Name:</label><br />
                            <input
                                type="text"
                                required
                                maxLength="55"
                                spellCheck="false"
                                autoComplete="off"
                                className="input"
                                style={{ transform: "translateY(-3px)", marginBottom: "4px" }}
                                onChange={this.handleFieldChange}
                                id="lastName"
                                value={this.state.lastName}
                            />
                        </div>
                        <div>
                            <label htmlFor="DepartmentID" className="fieldTitle">Department ID:</label><br />
                            <input
                                type="number"
                                required
                                spellCheck="false"
                                autoComplete="off"
                                min="1"
                                className="input"
                                style={{ transform: "translateY(-3px)" }}
                                onChange={this.handleFieldChange}
                                id="departmentId"
                                value={this.state.departmentId}
                            />
                        </div>
                        <div>
                            <label htmlFor="IsSupervisor" className="fieldTitle">Is Supervisor:</label><br />
                            <select id="isSuperVisor" className="dropDown" onChange={this.handleBoolChange} value={this.state.isSuperVisor}>
                                <option className="dropDownItem" value="true">Yes</option>
                                <option className="dropDownItem" value="false">No</option>
                            </select>
                        </div>

                        <div onClick={this.addEmployee} className="submitButton">Submit</div>
                    </div>
                    <NavButton text={"Return"} history={this.props.history} path={"/employees"} />
                </div>
            </React.Fragment>
        );
    }
}

export default EmployeesAdd