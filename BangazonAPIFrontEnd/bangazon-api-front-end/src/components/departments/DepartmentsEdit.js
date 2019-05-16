import React, { Component } from "react"
import DepartmentManager from "../../modules/DepartmentManager"
import NavButton from "../navButton/NavButton"

class DepartmentsEdit extends Component {
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

    updateDepartment = evt => {
        evt.preventDefault()

        const editedDepartment = {
            id: parseInt(this.props.match.params.departmentId),
            name: this.state.name,
            budget: parseInt(this.state.budget)
        }

        this.props.updateDepartments(editedDepartment)
            .then(() => this.props.history.push("/departments"))
    }

    componentDidMount() {
        DepartmentManager.get(`${this.props.match.params.departmentId}`)
            .then(department => {
                this.setState({
                    name: department.name,
                    budget: department.budget
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
                                style={{ transform: "translateY(-3px)", marginBottom: "4px" }}
                                onChange={this.handleFieldChange}
                                id="name"
                                value={this.state.name}
                            />
                        </div>
                        <div>
                            <label htmlFor="Budget" className="fieldTitle">Budget:</label><br />
                            <input
                                type="number"
                                required
                                spellCheck="false"
                                autoComplete="off"
                                className="input"
                                style={{ transform: "translateY(-3px)" }}
                                onChange={this.handleFieldChange}
                                id="budget"
                                value={this.state.budget}
                            />
                        </div>

                        <div onClick={this.updateDepartment} className="submitButton">Submit</div>
                    </div>
                    <NavButton text={"Return"} history={this.props.history} path={"/departments"} />
                </div>
            </React.Fragment>
        );
    }
}

export default DepartmentsEdit