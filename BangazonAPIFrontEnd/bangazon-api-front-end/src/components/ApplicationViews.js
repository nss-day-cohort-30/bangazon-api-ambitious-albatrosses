import React, { Component } from "react"
import { withRouter } from 'react-router'
import { Route } from "react-router-dom"
import ChooseResource from "./chooseResource/ChooseResource"
import EmployeesView from "./employees/EmployeesView"
import EmployeesEdit from "./employees/EmployeesEdit"
import EmployeesAdd from "./employees/EmployeesAdd"
import EmployeeManager from "../modules/EmployeeManager"

class ApplicationViews extends Component {

    state = {
        employees: []
    }

    updateEmployees = (editedEmployeeObject) => {
        return EmployeeManager.put(editedEmployeeObject)
          .then(() => EmployeeManager.getAll())
          .then(employees => {
            this.setState({
              employees: employees
            })
          })
      }

    addEmployees = (editedEmployeeObject) => {
        return EmployeeManager.post(editedEmployeeObject)
          .then(() => EmployeeManager.getAll())
          .then(employees => {
            this.setState({
              employees: employees
            })
          })
      }

    componentDidMount() {

        const newState = {
            employees: []
        }

        EmployeeManager.getAll()
            .then(employees => newState.employees = employees)
            .then(() => this.setState(newState))

    }

    render() {
        return (
            <React.Fragment>
                <Route exact path="/" render={props => {
                    return <ChooseResource history={this.props.history} />
                }} />
                <Route exact path="/employees" render={props => {
                    return <EmployeesView history={this.props.history} employees={this.state.employees} />
                }} />
                <Route
                    exact path="/employees/:employeeId(\d+)/edit" render={props => {
                        return <EmployeesEdit {...props} history={this.props.history} updateEmployees={this.updateEmployees} employees={this.state.employees} />
                    }}
                />
                <Route
                    exact path="/employees/add" render={props => {
                        return <EmployeesAdd {...props} history={this.props.history} addEmployees={this.addEmployees} />
                    }}
                />
            </React.Fragment>
        )
    }
}

export default withRouter(ApplicationViews)