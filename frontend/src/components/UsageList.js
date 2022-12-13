import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import CircularProgress from "@mui/material/CircularProgress";
import CenteredBox from "../components/CenteredBox";
import ErrorIcon from "@mui/icons-material/Error";
import ErrorBoundary from "./ErrorBoundary";
import LoadingBoundary from "./LoadingBoundary";
import UsageSubList from "./UsageSubList";

const UsageList = () => {
  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        mb: 3,
      }}
    >
      <Typography
        variant="h6"
        sx={{ flexGrow: 1, pb: 2, borderBottom: "2px solid #ededed", mb: 3 }}
      >
        Usage
      </Typography>

      <Box
        sx={{
          display: "flex",
          flexDirection: "row",
          gap: "10px",
        }}
      >
        <ErrorBoundary
          fallback={({ error }) => (
            <CenteredBox>
              <ErrorIcon sx={{ fontSize: 50, mb: 2 }} />
              <Typography
                variant="h7"
                sx={{ userSelect: "none", textAlign: "center" }}
              >
                {error}
              </Typography>
            </CenteredBox>
          )}
        >
          <LoadingBoundary
            fallback={
              <CenteredBox>
                <CircularProgress />
              </CenteredBox>
            }
          >
            <UsageSubList />
          </LoadingBoundary>
        </ErrorBoundary>
      </Box>
    </Box>
  );
};

export default UsageList;
