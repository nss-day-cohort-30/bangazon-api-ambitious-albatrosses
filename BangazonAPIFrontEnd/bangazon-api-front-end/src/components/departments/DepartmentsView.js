import React, { Component } from 'react'
import NavButton from "../navButton/NavButton"


class DepartmentsView extends Component {

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

    formatNumber(num) {
        return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
    }

    render() {
        return (
            <React.Fragment>
                <div className="resourceView">
                    <div className="messageText">Departments</div>
                    <input type="text" spellCheck="false" autoComplete="off" className="input" onChange={this.handleNameFieldChange} placeholder="Search name"></input>
                    <input type="number" min="1" spellCheck="false" autoComplete="off" className="input" onChange={this.handleIdFieldChange} placeholder="Search ID"></input>
                    <div className="resourceContainer">
                        {
                            this.props.departments
                                .filter(d => d.name.toUpperCase().includes(this.state.nameSearchTerms.toUpperCase()))
                                .filter(d => (this.state.idSearchTerms !== null) ? d.id === this.state.idSearchTerms : d)
                                .map(department => {
                                    return (
                                        <div key={department.id} className="resource">
                                            <div><span className="darkerText">ID: </span><strong>{department.id}</strong></div>
                                            <div><span className="darkerText">Name: </span><strong>{department.name}</strong></div>
                                            <div><span className="darkerText">Budget: </span><strong>${this.formatNumber(parseInt(department.budget))}</strong></div>
                                            <div><span className="darkerText">Employees: </span>
                                                {
                                                    department.employees.map(employee => { return <div key={employee.id}> - {employee.firstName} {employee.lastName} </div> })
                                                }
                                            </div>
                                            <div className="editButton" onClick={() => this.props.history.push("/departments/" + department.id + "/edit")}>Edit</div>
                                        </div>
                                    )
                                })
                        }
                    </div>
                    <NavButton text={"Return"} history={this.props.history} path={"/"} />
                    <NavButton text={"Add New Department"} history={this.props.history} path={"/departments/add"} />
                </div>
            </React.Fragment>
        )
    }
}

export default DepartmentsView