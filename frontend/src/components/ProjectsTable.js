import { useProjects } from "../hooks/useProjects";
import { useState } from "react";
import { NavLink } from "react-router-dom";
import Box from "@mui/material/Box";
import Container from "@mui/material/Container";
import Typography from "@mui/material/Typography";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TableContainer from "@mui/material/TableContainer";
import Link from "@mui/material/Link";
import CenteredBox from "../components/CenteredBox";
import Tooltip from "@mui/material/Tooltip";
import AccountTreeRoundedIcon from "@mui/icons-material/AccountTreeRounded";
import Moment from "react-moment";
import CreateProjectButton from "../components/CreateProjectButton";
import ProjectMenuButton from "../components/ProjectMenuButton";
import SearchBar from "../components/SearchBar";

const ProjectsTable = () => {
  const projects = useProjects();
  const [search, setSearch] = useState("");

  return (
    <>
      <Container sx={{ mt: 3 }}>
        <Box
          sx={{
            display: "flex",
            flexDirection: "row",
            mb: 2,
          }}
        >
          <SearchBar
            title="Projects"
            search={search}
            setSearch={setSearch}
            element={
              <CreateProjectButton
                icon
                sx={{
                  fontSize: 13,
                  background: "#eaecf0",
                  color: "black",
                  "&:hover": { background: "#d5d7db" },
                }}
              />
            }
          />
        </Box>

        <TableContainer
          sx={{
            border: "1px solid #e0e0e0",
            mb: 2,
            borderRadius: 1,
          }}
          component={Box}
        >
          <Table>
            <TableHead sx={{ pb: 1 }}>
              <TableRow>
                <TableCell align="left">Name</TableCell>
                <TableCell align="left">Created</TableCell>
                <TableCell align="left">Organizations</TableCell>
                {/* <TableCell align="right"></TableCell> */}
              </TableRow>
            </TableHead>

            {projects.length > 0 && (
              <TableBody>
                {projects
                  .filter((project) =>
                    project.name.toLowerCase().includes(search.toLowerCase())
                  )
                  .map((project) => (
                    <TableRow
                      key={project.id}
                      sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
                    >
                      <TableCell align="left">
                        <Link
                          component={NavLink}
                          to={"/projects/" + project.id}
                          sx={{
                            display: "flex",
                            flexDirection: "row",
                            alignItems: "center",
                          }}
                        >
                          <AccountTreeRoundedIcon sx={{ mr: 3 }} />
                          {project.name}
                        </Link>
                      </TableCell>
                      <TableCell align="left">
                        <Tooltip title={project.createdAt}>
                          <Box>
                            <Moment fromNow ago>
                              {project.createdAt}
                            </Moment>{" "}
                            ago
                          </Box>
                        </Tooltip>
                      </TableCell>
                      <TableCell align="right">
                        <ProjectMenuButton project={project} />
                      </TableCell>
                    </TableRow>
                  ))}
              </TableBody>
            )}
          </Table>

          {projects.length === 0 && (
            <CenteredBox sx={{ py: 5 }}>
              <Typography variant="h7" sx={{ color: "gray" }}>
                You currently have no projects.
              </Typography>
            </CenteredBox>
          )}
        </TableContainer>
      </Container>
    </>
  );
};

export default ProjectsTable;
