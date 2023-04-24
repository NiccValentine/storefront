import { Delete, KeyboardArrowLeft, Save } from "@mui/icons-material";
import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Divider, FormControl, FormGroup, IconButton, TextField, Toolbar, Typography, useTheme, } from "@mui/material";
import { Box, width } from "@mui/system";
import React from "react";
import { useNavigate, useParams } from "react-router-dom";
import useMessageProvider from "../../../shared/hooks/messageprovider/useMessageProvider";
import ProductService from "../../../shared/services/productService";


const productService = new ProductService();

const ProductEdit = () => {

    const { addMessage } = useMessageProvider();
    const { addError } = useMessageProvider();
    const theme = useTheme();
    const navigate = useNavigate();
    const { productId } = useParams();

    const [product, setProduct] = React.useState({
        productId: null,
        productName: "",
        productDescription: ""
    })

    const [errors, setErrors] = React.useState([])
    const [openDelete, setOpenDelete] = React.useState(false)

    React.useEffect(() => {
        productService.getSingle(productId).then(response => {
            if (response.status === 200) {
                setProduct(response.data)
            }
        })
    }, []);

    const navBack = () => {
        if (product.productId === null) {
            addError("Add product cancelled")
            navigate("/products")
        }
        else {
            navigate(`/products/${productId}`)
        }
    }

    const handleChange = (property, newValue) => {
        setProduct({ ...product, [property]: newValue })
    }

    const handleDeleteOpen = () => {
        setOpenDelete(true);
    }

    const handleDeleteClose = () => {
        setOpenDelete(false);
    }

    const handleDeleteAccept = () => {
        product.productId = productId
        productService.delete(productId)
            .then((response) => {
                if (response.status === 200) {
                    addError("Delete Successful")
                    navigate("/products");
                }
            }).catch((error) => {
                if (error.response.status === 500) {
                    setErrors(error.response.data.messages)
                    addError("Unable to delete this product, please remove it from any stores it is tied to")
                }
            });
        setOpenDelete(false);
    }

    const saveProduct = () => {
        if (product.productId === null || product.productId === undefined) {
            product.productId = productId
            productService.post(product)
                .then((response) => {
                    if (response.status === 201) {
                        addMessage("Save successful!")
                        navigate(`/products/${productId}`)
                    }
                }).catch((error) => {
                    if (error.response.status === 400) {
                        addError("There were validation errors")
                        product.productId = null
                        setErrors(error.response.data.messages)
                    }
                });
        }
        else {
            productService.put(product, productId)
                .then((response) => {
                    if (response.status === 200) {
                        addMessage("Save successful!")
                        navigate(`/products/${productId}`)
                    }
                }).catch((error) => {
                    if (error.response.status === 400) {
                        addError("There were validation errors")
                        setErrors(error.response.data.messages)
                    }
                });
        }
    }

    return (
        <React.Fragment>
            <Toolbar>
                <IconButton onClick={navBack} color="inherit">
                    <KeyboardArrowLeft fontSize="large" />
                </IconButton>
                <Typography variant="h3">
                    Edit Product
                </Typography>
                <Box sx={{
                    flexGrow: 1
                }} />
                <IconButton onClick={saveProduct}>
                    <Save fontSize="large" />
                </IconButton>
                <IconButton onClick={() => handleDeleteOpen()}>
                    <Delete fontSize="large" />
                </IconButton>
                <Dialog
                    open={openDelete}
                    onClose={handleDeleteClose}
                >
                    <DialogTitle id="delete-dialog-description">
                        {"Delete Product"}
                    </DialogTitle>
                    <DialogContent>
                        <DialogContentText>
                            <Typography>
                                {product.productName} will be deleted.
                            </Typography>
                        </DialogContentText>
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={handleDeleteClose}>Cancel</Button>
                        <Button onClick={handleDeleteAccept}>Accept</Button>
                    </DialogActions>
                </Dialog>
            </Toolbar>
            <Divider />
            <FormGroup>
                <FormControl sx={{
                    minWidth: '200px',
                    padding: theme.spacing(1)

                }}>
                    <TextField
                        label="Product Name"
                        value={product.productName}
                        onChange={event => handleChange("productName", event.target.value)}
                        error={errors && errors.find(message => message.fieldName === 'ProductName') !== undefined}
                        helperText={errors && errors.find(message => message.fieldName === 'ProductName')?.messageText}
                    />
                </FormControl>
                <FormControl sx={{
                    minWidth: '200px',
                    padding: theme.spacing(1)

                }}>
                    <TextField
                        label="Product Description"
                        value={product.productDescription}
                        onChange={event => handleChange("productDescription", event.target.value)}
                        multiline
                        minRows={3}
                        error={errors && errors.find(message => message.fieldName === 'ProductDescription') !== undefined}
                        helperText={errors && errors.find(message => message.fieldName === 'ProductDescription')?.messageText}
                    />
                </FormControl>
            </FormGroup>

        </React.Fragment>
    )
}

export default ProductEdit;