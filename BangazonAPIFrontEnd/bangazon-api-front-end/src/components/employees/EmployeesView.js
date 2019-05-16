import React, { Component } from 'react'
import NavButton from "../navButton/NavButton"


class EmployeesView extends Component {

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

    convertBoolToEnglish = (bool) => {
        if (bool === "true") {
            return "Yes"
        }
        else {
            return "No"
        }
    }

    formatNumber(num) {
        return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
    }

    render() {
        return (
            <React.Fragment>
                <div className="resourceView">
                    <div className="messageText">Employees</div>
                    <input type="text" spellCheck="false" autoComplete="off" className="input" onChange={this.handleNameFieldChange} placeholder="Search name"></input>
                    <input type="number" min="1" spellCheck="false" autoComplete="off" className="input" onChange={this.handleIdFieldChange} placeholder="Search ID"></input>
                    <div className="resourceContainer">
                        {
                            this.props.employees
                                .filter(e => e.firstName.toUpperCase().includes(this.state.nameSearchTerms.toUpperCase()) || e.lastName.toUpperCase().includes(this.state.nameSearchTerms.toUpperCase()))
                                .filter(e => (this.state.idSearchTerms !== null) ? e.id === this.state.idSearchTerms : e)
                                .map(employee => {
                                    return (
                                        employee.computer !== null ?
                                            <div key={employee.id} className="resource">
                                                <div><span className="darkerText">ID: </span><strong>{employee.id}</strong></div>
                                                <div><span className="darkerText">Name: </span><strong>{employee.firstName} {employee.lastName}</strong></div>
                                                <div><span className="darkerText">Supervisor: </span><strong>{this.convertBoolToEnglish(employee.isSuperVisor.toString())}</strong></div>
                                                <div><span className="darkerText">Department Name: </span><strong>{employee.department.name}</strong></div>
                                                <div><span className="darkerText">Department Budget: </span><strong>${this.formatNumber(parseInt(employee.department.budget))}</strong></div>
                                                <div><span className="darkerText">Computer: </span><strong>{employee.computer.manufacturer} {employee.computer.make}</strong></div>
                                                <div className="editButton" onClick={() => this.props.history.push("/employees/" + employee.id + "/edit")}>Edit</div>
                                            </div> :
                                            <div key={employee.id} className="resource">
                                                <div><span className="darkerText">ID: </span><strong>{employee.id}</strong></div>
                                                <div><span className="darkerText">Name: </span><strong>{employee.firstName} {employee.lastName}</strong></div>
                                                <div><span className="darkerText">Supervisor: </span><strong>{this.convertBoolToEnglish(employee.isSuperVisor.toString())}</strong></div>
                                                <div><span className="darkerText">Department Name: </span><strong>{employee.department.name}</strong></div>
                                                <div><span className="darkerText">Department Budget: </span><strong>${this.formatNumber(parseInt(employee.department.budget))}</strong></div>
                                                <div><span className="darkerText">Computer: </span><strong>N/A</strong></div>
                                                <div className="editButton" onClick={() => this.props.history.push("/employees/" + employee.id + "/edit")}>Edit</div>
                                            </div>
                                    )
                                })
                        }
                    </div>
                    <NavButton text={"Return"} history={this.props.history} path={"/"} />
                    <NavButton text={"Add New Employee"} history={this.props.history} path={"/employees/add"} />
                </div>
            </React.Fragment>
        )
    }
}

export default EmployeesView