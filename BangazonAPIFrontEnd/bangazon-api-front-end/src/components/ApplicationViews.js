import React, { Component } from "react"
import { withRouter } from 'react-router'
import { Route } from "react-router-dom"
import ChooseResource from "./chooseResource/ChooseResource"
import EmployeesView from "./employees/EmployeesView"
import EmployeesEdit from "./employees/EmployeesEdit"
import EmployeesAdd from "./employees/EmployeesAdd"
import EmployeeManager from "../modules/EmployeeManager"
import DepartmentsView from "./departments/DepartmentsView"
import DepartmentsEdit from "./departments/DepartmentsEdit"
import DepartmentsAdd from "./departments/DepartmentsAdd"
import DepartmentManager from "../modules/DepartmentManager"
import ProductsView from "./products/ProductsView"
import ProductsEdit from "./products/ProductsEdit"
import ProductsAdd from "./products/ProductsAdd"
import ProductManager from "../modules/ProductManager"
import ProductTypesView from "./productTypes/ProductTypesView"
import ProductTypesEdit from "./productTypes/ProductTypesEdit"
import ProductTypesAdd from "./productTypes/ProductTypesAdd"
import ProductTypeManager from "../modules/ProductTypeManager"

class ApplicationViews extends Component {

    state = {
        employees: [],
        departments: [],
        products: [],
        productTypes: []
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

    updateDepartments = (editedDepartmentObject) => {
        return DepartmentManager.put(editedDepartmentObject)
            .then(() => DepartmentManager.getAll())
            .then(departments => {
                this.setState({
                    departments: departments
                })
            })
    }

    addDepartments = (editedDepartmentObject) => {
        return DepartmentManager.post(editedDepartmentObject)
            .then(() => DepartmentManager.getAll())
            .then(departments => {
                this.setState({
                    departments: departments
                })
            })
    }

    updateProducts = (editedProductObject) => {
        return ProductManager.put(editedProductObject)
            .then(() => ProductManager.getAll())
            .then(products => {
                this.setState({
                    products: products
                })
            })
    }

    addProducts = (editedProductObject) => {
        return ProductManager.post(editedProductObject)
            .then(() => ProductManager.getAll())
            .then(products => {
                this.setState({
                    products: products
                })
            })
    }

    updateProductTypes = (editedProductTypeObject) => {
        return ProductTypeManager.put(editedProductTypeObject)
            .then(() => ProductTypeManager.getAll())
            .then(productTypes => {
                this.setState({
                    productTypes: productTypes
                })
            })
    }

    addProductTypes = (editedProductTypeObject) => {
        return ProductTypeManager.post(editedProductTypeObject)
            .then(() => ProductTypeManager.getAll())
            .then(productTypes => {
                this.setState({
                    productTypes: productTypes
                })
            })
    }

    componentDidMount() {

        const newState = {
            employees: [],
            departments: [],
            products: [],
            productTypes: []
        }

        EmployeeManager.getAll()
            .then(employees => newState.employees = employees)
            .then(DepartmentManager.getAll)
            .then(departments => newState.departments = departments)
            .then(ProductManager.getAll)
            .then(products => newState.products = products)
            .then(ProductTypeManager.getAll)
            .then(productTypes => newState.productTypes = productTypes)
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
                <Route exact path="/departments" render={props => {
                    return <DepartmentsView history={this.props.history} departments={this.state.departments} />
                }} />
                <Route
                    exact path="/departments/:departmentId(\d+)/edit" render={props => {
                        return <DepartmentsEdit {...props} history={this.props.history} updateDepartments={this.updateDepartments} departments={this.state.departments} />
                    }}
                />
                <Route
                    exact path="/departments/add" render={props => {
                        return <DepartmentsAdd {...props} history={this.props.history} addDepartments={this.addDepartments} />
                    }}
                />
                <Route exact path="/products" render={props => {
                    return <ProductsView history={this.props.history} products={this.state.products} />
                }} />
                <Route
                    exact path="/products/:productId(\d+)/edit" render={props => {
                        return <ProductsEdit {...props} history={this.props.history} updateProducts={this.updateProducts} products={this.state.products} />
                    }}
                />
                <Route
                    exact path="/products/add" render={props => {
                        return <ProductsAdd {...props} history={this.props.history} addProducts={this.addProducts} />
                    }}
                />
                <Route exact path="/productTypes" render={props => {
                    return <ProductTypesView history={this.props.history} productTypes={this.state.productTypes} />
                }} />
                <Route
                    exact path="/productTypes/:productTypeId(\d+)/edit" render={props => {
                        return <ProductTypesEdit {...props} history={this.props.history} updateProductTypes={this.updateProductTypes} productTypes={this.state.productTypes} />
                    }}
                />
                <Route
                    exact path="/productTypes/add" render={props => {
                        return <ProductTypesAdd {...props} history={this.props.history} addProductTypes={this.addProductTypes} />
                    }}
                />
            </React.Fragment>
        )
    }
}

export default withRouter(ApplicationViews)